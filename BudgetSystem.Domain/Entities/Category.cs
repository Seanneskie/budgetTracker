using BudgetSystem.Domain.Common;
using BudgetSystem.Domain.Enums;

namespace BudgetSystem.Domain.Entities;

public class Category : EntityBase
{
    public string Name { get; set; } = default!;
    public TransactionType Type { get; set; }  // Income or Expense (usually)
    public bool IsArchived { get; set; }

    // Optional: per-account scoping (uncomment if you want categories per account)
    public int? AccountId { get; set; }
    public Account? Account { get; set; }
}
