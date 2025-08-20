using BudgetSystem.Application.DTOs;
using BudgetSystem.Domain.Entities;

namespace BudgetSystem.Application.Mappers;

public static class BudgetMappers
{
    public static Budget ToEntity(this BudgetCreateDto dto) => new()
    {
        Year = dto.Year,
        Month = dto.Month,
        LimitAmount = dto.LimitAmount,
        AccountId = dto.AccountId,
        CategoryId = dto.CategoryId
    };

    public static void MapTo(this BudgetUpdateDto dto, Budget entity)
    {
        entity.LimitAmount = dto.LimitAmount;
        entity.CategoryId = dto.CategoryId;
        entity.UpdatedUtc = DateTime.UtcNow;
    }
}
