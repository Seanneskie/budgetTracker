using BudgetSystem.Application.DTOs;
using BudgetSystem.Domain.Entities;

namespace BudgetSystem.Application.Mappers;

public static class CategoryMappers
{
    public static Category ToEntity(this CategoryCreateDto dto) => new()
    {
        Name = dto.Name,
        Type = dto.Type,
        AccountId = dto.AccountId
    };

    public static void MapTo(this CategoryUpdateDto dto, Category entity)
    {
        entity.Name = dto.Name;
        entity.Type = dto.Type;
        entity.IsArchived = dto.IsArchived;
        entity.UpdatedUtc = DateTime.UtcNow;
    }
}
