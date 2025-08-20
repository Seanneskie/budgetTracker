using BudgetSystem.Application.DTOs;
using BudgetSystem.Domain.Entities;

namespace BudgetSystem.Application.Mappers;

public static class AccountMappers
{
    public static Account ToEntity(this AccountCreateDto dto) => new()
    {
        Name = dto.Name,
        Currency = dto.Currency,
        StartingBalance = dto.StartingBalance
    };

    public static void MapTo(this AccountUpdateDto dto, Account entity)
    {
        entity.Name = dto.Name;
        entity.Currency = dto.Currency;
        entity.UpdatedUtc = DateTime.UtcNow;
    }
}
