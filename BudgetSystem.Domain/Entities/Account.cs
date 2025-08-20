using BudgetSystem.Domain.Common;

namespace BudgetSystem.Domain.Entities;

public class Account : EntityBase
{
    public string Name { get; set; } = default!;
    public decimal StartingBalance { get; set; }
    public string Currency { get; set; } = "PHP";

    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
