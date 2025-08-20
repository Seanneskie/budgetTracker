using BudgetSystem.Domain.Common;

namespace BudgetSystem.Domain.Entities;

public class Budget : EntityBase
{
    public int Year { get; set; }
    public int Month { get; set; } // 1-12

    public decimal LimitAmount { get; set; }

    public int AccountId { get; set; }
    public Account Account { get; set; } = default!;

    public int? CategoryId { get; set; } // optional category-level budgets
    public Category? Category { get; set; }
}
