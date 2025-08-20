namespace BudgetSystem.Infrastructure.Idempotency;

public class IdempotencyRecord
{
    public int Id { get; set; }
    public string Key { get; set; } = default!;
    public string RequestHash { get; set; } = default!;
    public string Method { get; set; } = default!;
    public string Path { get; set; } = default!;
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime ExpiresUtc { get; set; } = DateTime.UtcNow.AddHours(24);
}
