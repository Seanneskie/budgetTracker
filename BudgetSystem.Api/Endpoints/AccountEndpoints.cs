using BudgetSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using BudgetSystem.Application.DTOs;
using BudgetSystem.Application.Mappers;
using FluentValidation;

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

        g.MapPost("/", async (
            AccountCreateDto dto,
            IValidator<AccountCreateDto> validator,
            AppDbContext db) =>
        {
            var validation = await validator.ValidateAsync(dto);
            if (!validation.IsValid)
                return Results.ValidationProblem(validation.ToDictionary());

            var entity = dto.ToEntity();
            db.Accounts.Add(entity);
            await db.SaveChangesAsync();

            var result = new
            {
                entity.Id,
                entity.Name,
                entity.StartingBalance,
                entity.Currency,
                entity.CreatedUtc,
                entity.UpdatedUtc
            };

            return Results.Created($"/api/v1/accounts/{entity.Id}", result);
        });

        g.MapPut("/{id:int}", async (
            int id,
            AccountUpdateDto dto,
            IValidator<AccountUpdateDto> validator,
            AppDbContext db) =>
        {
            var validation = await validator.ValidateAsync(dto);
            if (!validation.IsValid)
                return Results.ValidationProblem(validation.ToDictionary());

            var entity = await db.Accounts.FindAsync(id);
            if (entity is null)
                return Results.NotFound();

            dto.MapTo(entity);
            await db.SaveChangesAsync();

            return Results.NoContent();
        });

        g.MapDelete("/{id:int}", async (int id, AppDbContext db) =>
        {
            var entity = await db.Accounts.FindAsync(id);
            if (entity is null)
                return Results.NotFound();

            db.Accounts.Remove(entity);
            await db.SaveChangesAsync();

            return Results.NoContent();
        });

        return app;
    }
}
