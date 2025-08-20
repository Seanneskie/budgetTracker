using BudgetSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BudgetSystem.Api.Endpoints;

public static class AccountEndpoints
{
    public static IEndpointRouteBuilder MapAccountEndpoints(this IEndpointRouteBuilder app)
    {
        var g = app.MapGroup("/api/v1/accounts").WithTags("Accounts");

        g.MapGet("/", async (AppDbContext db) =>
            await db.Accounts.AsNoTracking()
                .Select(a => new { a.Id, a.Name, a.StartingBalance, a.Currency, a.CreatedUtc, a.UpdatedUtc })
                .ToListAsync());

        g.MapGet("/{id:int}", async (int id, AppDbContext db) =>
        {
            var item = await db.Accounts.AsNoTracking()
                .Where(a => a.Id == id)
                .Select(a => new { a.Id, a.Name, a.StartingBalance, a.Currency, a.CreatedUtc, a.UpdatedUtc })
                .FirstOrDefaultAsync();
            return item is null ? Results.NotFound() : Results.Ok(item);
        });

        return app;
    }
}
