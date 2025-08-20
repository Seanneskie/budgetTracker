using BudgetSystem.Application.DTOs;
using BudgetSystem.Domain.Entities;

namespace BudgetSystem.Application.Mappers;

public static class TransactionMappers
{
    public static Transaction ToEntity(this TransactionCreateDto dto) => new()
    {
        Date = dto.Date,
        Amount = dto.Amount,
        Notes = dto.Notes,
        Type = dto.Type,
        AccountId = dto.AccountId,
        CategoryId = dto.CategoryId,
        FromAccountId = dto.FromAccountId,
        ToAccountId = dto.ToAccountId
    };

    public static void MapTo(this TransactionUpdateDto dto, Transaction entity)
    {
        entity.Date = dto.Date;
        entity.Amount = dto.Amount;
        entity.Notes = dto.Notes;
        entity.Type = dto.Type;
        entity.AccountId = dto.AccountId;
        entity.CategoryId = dto.CategoryId;
        entity.FromAccountId = dto.FromAccountId;
        entity.ToAccountId = dto.ToAccountId;
        entity.UpdatedUtc = DateTime.UtcNow;
    }
}
