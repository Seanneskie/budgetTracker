using BudgetSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BudgetSystem.Api.Endpoints;

public static class BudgetEndpoints
{
    public static IEndpointRouteBuilder MapBudgetEndpoints(this IEndpointRouteBuilder app)
    {
        var g = app.MapGroup("/api/v1/budgets").WithTags("Budgets");

        g.MapGet("/", async (AppDbContext db) =>
            await db.Budgets.AsNoTracking()
                .Select(b => new
                {
                    b.Id, b.Year, b.Month, b.LimitAmount,
                    b.AccountId, b.CategoryId, b.CreatedUtc, b.UpdatedUtc
                })
                .ToListAsync());

        g.MapGet("/{id:int}", async (int id, AppDbContext db) =>
        {
            var item = await db.Budgets.AsNoTracking()
                .Where(b => b.Id == id)
                .Select(b => new
                {
                    b.Id, b.Year, b.Month, b.LimitAmount,
                    b.AccountId, b.CategoryId, b.CreatedUtc, b.UpdatedUtc
                })
                .FirstOrDefaultAsync();

            return item is null ? Results.NotFound() : Results.Ok(item);
        });

        return app;
    }
}
