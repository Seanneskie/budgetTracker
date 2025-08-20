using BudgetSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BudgetSystem.Api.Endpoints;

public static class CategoryEndpoints
{
    public static IEndpointRouteBuilder MapCategoryEndpoints(this IEndpointRouteBuilder app)
    {
        var g = app.MapGroup("/api/v1/categories").WithTags("Categories");

        g.MapGet("/", async (AppDbContext db) =>
            await db.Categories.AsNoTracking()
                .Select(c => new { c.Id, c.Name, c.Type, c.AccountId, c.IsArchived, c.CreatedUtc, c.UpdatedUtc })
                .ToListAsync());

        g.MapGet("/{id:int}", async (int id, AppDbContext db) =>
        {
            var item = await db.Categories.AsNoTracking()
                .Where(c => c.Id == id)
                .Select(c => new { c.Id, c.Name, c.Type, c.AccountId, c.IsArchived, c.CreatedUtc, c.UpdatedUtc })
                .FirstOrDefaultAsync();

            return item is null ? Results.NotFound() : Results.Ok(item);
        });

        return app;
    }
}
