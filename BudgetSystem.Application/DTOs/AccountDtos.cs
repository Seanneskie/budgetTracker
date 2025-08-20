namespace BudgetSystem.Application.DTOs;

public record AccountCreateDto(string Name, decimal StartingBalance, string Currency = "PHP");
public record AccountUpdateDto(string Name, string Currency = "PHP");
