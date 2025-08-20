using BudgetSystem.Domain.Enums;

namespace BudgetSystem.Application.DTOs;

public record CategoryCreateDto(string Name, TransactionType Type, int? AccountId);
public record CategoryUpdateDto(string Name, TransactionType Type, bool IsArchived);
