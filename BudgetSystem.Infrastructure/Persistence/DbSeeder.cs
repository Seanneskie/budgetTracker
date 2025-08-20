using BudgetSystem.Domain.Entities;
using BudgetSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace BudgetSystem.Infrastructure.Persistence;

public static class DbSeeder
{
    public static async Task EnsureSeededAsync(this AppDbContext db)
    {
        if (!await db.Accounts.AnyAsync())
        {
            db.Accounts.AddRange(
                new Account { Name = "Cash Wallet", Currency = "PHP", StartingBalance = 0m },
                new Account { Name = "BPI Checking", Currency = "PHP", StartingBalance = 0m }
            );
        }

        if (!await db.Categories.AnyAsync())
        {
            db.Categories.AddRange(
                new Category { Name = "Salary", Type = TransactionType.Income },
                new Category { Name = "Groceries", Type = TransactionType.Expense },
                new Category { Name = "Transport", Type = TransactionType.Expense }
            );
        }

        await db.SaveChangesAsync();
    }
}
