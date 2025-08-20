using BudgetSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BudgetSystem.Api.Endpoints;

public static class TransactionEndpoints
{
    public static IEndpointRouteBuilder MapTransactionEndpoints(this IEndpointRouteBuilder app)
    {
        var g = app.MapGroup("/api/v1/transactions").WithTags("Transactions");

        g.MapGet("/", async (AppDbContext db) =>
            await db.Transactions.AsNoTracking()
                .OrderByDescending(t => t.Date)
                .Select(t => new
                {
                    t.Id, t.Date, t.Amount, t.Type, t.Notes,
                    t.AccountId, t.CategoryId, t.FromAccountId, t.ToAccountId,
                    t.CreatedUtc, t.UpdatedUtc
                })
                .ToListAsync());

        g.MapGet("/{id:int}", async (int id, AppDbContext db) =>
        {
            var item = await db.Transactions.AsNoTracking()
                .Where(t => t.Id == id)
                .Select(t => new
                {
                    t.Id, t.Date, t.Amount, t.Type, t.Notes,
                    t.AccountId, t.CategoryId, t.FromAccountId, t.ToAccountId,
                    t.CreatedUtc, t.UpdatedUtc
                })
                .FirstOrDefaultAsync();

            return item is null ? Results.NotFound() : Results.Ok(item);
        });

        return app;
    }
}
