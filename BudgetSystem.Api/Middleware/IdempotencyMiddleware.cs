using System.Security.Cryptography;
using System.Text;
using BudgetSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BudgetSystem.Api.Middleware;

public class IdempotencyMiddleware
{
    private readonly RequestDelegate _next;
    public IdempotencyMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext ctx, AppDbContext db)
    {
        // Only for state-changing methods
        if (HttpMethods.IsGet(ctx.Request.Method))
        {
            await _next(ctx);
            return;
        }

        if (!ctx.Request.Headers.TryGetValue("Idempotency-Key", out var key) || string.IsNullOrWhiteSpace(key))
        {
            // No key provided -> proceed normally
            await _next(ctx);
            return;
        }

        // Read body to compute a stable hash
        ctx.Request.EnableBuffering();
        string body;
        using (var reader = new StreamReader(ctx.Request.Body, Encoding.UTF8, leaveOpen: true))
        {
            body = await reader.ReadToEndAsync();
            ctx.Request.Body.Position = 0;
        }

        var raw = $"{ctx.Request.Method}|{ctx.Request.Path}|{body}";
        string hash;
        using (var sha = SHA256.Create())
            hash = Convert.ToHexString(sha.ComputeHash(Encoding.UTF8.GetBytes(raw)));

        var path = ctx.Request.Path.ToString();

        var existing = await db.IdempotencyRecords
            .FirstOrDefaultAsync(i => i.Key == key && i.Path == path && i.RequestHash == hash && i.ExpiresUtc > DateTime.UtcNow);

        if (existing is not null)
        {
            // Identical request already seen -> reject as duplicate
            ctx.Response.StatusCode = StatusCodes.Status409Conflict;
            await Results.Problem(
                type: "https://httpstatuses.com/409",
                title: "Duplicate request",
                detail: "An identical request with this Idempotency-Key has already been processed.",
                statusCode: StatusCodes.Status409Conflict
            ).ExecuteAsync(ctx);
            return;
        }

        // Reserve the key (simple approach). TTL avoids permanent blocks on failures.
        db.IdempotencyRecords.Add(new Infrastructure.Idempotency.IdempotencyRecord
        {
            Key = key!,
            RequestHash = hash,
            Method = ctx.Request.Method,
            Path = path,
            CreatedUtc = DateTime.UtcNow,
            ExpiresUtc = DateTime.UtcNow.AddHours(24)
        });
        await db.SaveChangesAsync();

        await _next(ctx);
    }
}
