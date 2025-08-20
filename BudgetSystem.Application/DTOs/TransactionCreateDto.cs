using BudgetSystem.Domain.Enums;

namespace BudgetSystem.Application.DTOs;

public class TransactionCreateDto
{
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public decimal Amount { get; set; }
    public string? Notes { get; set; }
    public TransactionType Type { get; set; } // Income/Expense/Transfer
    public int AccountId { get; set; }        // used for Income/Expense
    public int? CategoryId { get; set; }      // optional for Income/Expense
    public int? FromAccountId { get; set; }   // required for Transfer
    public int? ToAccountId { get; set; }     // required for Transfer
}
