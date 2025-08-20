using BudgetSystem.Domain.Enums;

namespace BudgetSystem.Application.DTOs;

public class TransactionUpdateDto
{
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public decimal Amount { get; set; }
    public string? Notes { get; set; }
    public TransactionType Type { get; set; }
    public int AccountId { get; set; }
    public int? CategoryId { get; set; }
    public int? FromAccountId { get; set; }
    public int? ToAccountId { get; set; }
}
