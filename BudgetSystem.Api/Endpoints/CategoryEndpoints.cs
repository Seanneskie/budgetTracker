using BudgetSystem.Application.DTOs;
using BudgetSystem.Application.Mappers;
using BudgetSystem.Application.Validation;
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

        g.MapPost("/", async (CategoryCreateDto dto, AppDbContext db) =>
        {
            var validator = new CategoryCreateValidator();
            var validation = validator.Validate(dto);
            if (!validation.IsValid)
            {
                var errors = validation.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
                return Results.ValidationProblem(errors);
            }

            var entity = dto.ToEntity();
            db.Categories.Add(entity);
            await db.SaveChangesAsync();

            var result = new
            {
                entity.Id,
                entity.Name,
                entity.Type,
                entity.AccountId,
                entity.IsArchived,
                entity.CreatedUtc,
                entity.UpdatedUtc
            };
            return Results.Created($"/api/v1/categories/{entity.Id}", result);
        });

        g.MapPut("/{id:int}", async (int id, CategoryUpdateDto dto, AppDbContext db) =>
        {
            var validator = new CategoryUpdateValidator();
            var validation = validator.Validate(dto);
            if (!validation.IsValid)
            {
                var errors = validation.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
                return Results.ValidationProblem(errors);
            }

            var entity = await db.Categories.FindAsync(id);
            if (entity is null)
            {
                return Results.NotFound();
            }

            dto.MapTo(entity);
            await db.SaveChangesAsync();

            return Results.NoContent();
        });

        g.MapDelete("/{id:int}", async (int id, AppDbContext db) =>
        {
            var entity = await db.Categories.FindAsync(id);
            if (entity is null)
            {
                return Results.NotFound();
            }

            db.Categories.Remove(entity);
            await db.SaveChangesAsync();

            return Results.NoContent();
        });

        return app;
    }
}
