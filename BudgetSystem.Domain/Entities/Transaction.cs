using BudgetSystem.Domain.Common;
using BudgetSystem.Domain.Enums;

namespace BudgetSystem.Domain.Entities;

public class Transaction : EntityBase
{
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public decimal Amount { get; set; }
    public string? Notes { get; set; }

    public TransactionType Type { get; set; }  // Income/Expense/Transfer

    // Main account
    public int AccountId { get; set; }
    public Account Account { get; set; } = default!;

    // Category (null for Transfers if you prefer)
    public int? CategoryId { get; set; }
    public Category? Category { get; set; }

    // For Transfers (optional fields)
    public int? FromAccountId { get; set; }
    public int? ToAccountId { get; set; }
}
