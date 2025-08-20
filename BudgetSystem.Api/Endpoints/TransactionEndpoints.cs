using BudgetSystem.Application.DTOs;
using BudgetSystem.Application.Mappers;
using BudgetSystem.Infrastructure.Persistence;
using FluentValidation;
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

        g.MapPost("/", async (TransactionCreateDto dto, IValidator<TransactionCreateDto> validator, AppDbContext db) =>
        {
            var result = await validator.ValidateAsync(dto);
            if (!result.IsValid)
                return Results.ValidationProblem(result.ToDictionary());

            var entity = dto.ToEntity();
            db.Transactions.Add(entity);
            await db.SaveChangesAsync();

            return Results.Created($"/api/v1/transactions/{entity.Id}", new { entity.Id });
        });

        g.MapPut("/{id:int}", async (int id, TransactionUpdateDto dto, IValidator<TransactionUpdateDto> validator, AppDbContext db) =>
        {
            var result = await validator.ValidateAsync(dto);
            if (!result.IsValid)
                return Results.ValidationProblem(result.ToDictionary());

            var entity = await db.Transactions.FindAsync(id);
            if (entity is null) return Results.NotFound();

            dto.MapTo(entity);
            await db.SaveChangesAsync();

            return Results.NoContent();
        });

        g.MapDelete("/{id:int}", async (int id, AppDbContext db) =>
        {
            var entity = await db.Transactions.FindAsync(id);
            if (entity is null) return Results.NotFound();

            db.Transactions.Remove(entity);
            await db.SaveChangesAsync();

            return Results.NoContent();
        });

        return app;
    }
}
