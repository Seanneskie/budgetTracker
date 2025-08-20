namespace BudgetSystem.Application.DTOs;

public record BudgetCreateDto(int Year, int Month, decimal LimitAmount, int AccountId, int? CategoryId);
public record BudgetUpdateDto(decimal LimitAmount, int? CategoryId);
