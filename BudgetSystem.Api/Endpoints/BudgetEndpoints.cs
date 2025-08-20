using BudgetSystem.Application.DTOs;
using BudgetSystem.Application.Mappers;
using BudgetSystem.Infrastructure.Persistence;
using FluentValidation;
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

        g.MapPost("/", async (BudgetCreateDto dto, IValidator<BudgetCreateDto> validator, AppDbContext db) =>
        {
            var validation = await validator.ValidateAsync(dto);
            if (!validation.IsValid)
                return Results.ValidationProblem(validation.ToDictionary());

            var entity = dto.ToEntity();
            db.Budgets.Add(entity);
            await db.SaveChangesAsync();

            return Results.Created($"/api/v1/budgets/{entity.Id}", new
            {
                entity.Id, entity.Year, entity.Month, entity.LimitAmount,
                entity.AccountId, entity.CategoryId, entity.CreatedUtc, entity.UpdatedUtc
            });
        });

        g.MapPut("/{id:int}", async (int id, BudgetUpdateDto dto, IValidator<BudgetUpdateDto> validator, AppDbContext db) =>
        {
            var validation = await validator.ValidateAsync(dto);
            if (!validation.IsValid)
                return Results.ValidationProblem(validation.ToDictionary());

            var entity = await db.Budgets.FindAsync(id);
            if (entity is null)
                return Results.NotFound();

            dto.MapTo(entity);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        g.MapDelete("/{id:int}", async (int id, AppDbContext db) =>
        {
            var entity = await db.Budgets.FindAsync(id);
            if (entity is null)
                return Results.NotFound();

            db.Budgets.Remove(entity);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        return app;
    }
}
