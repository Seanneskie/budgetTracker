using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BudgetSystem.Infrastructure.Persistence;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        // Fallback connection (only used by design-time tools)
        var conn = "Server=localhost\\SQLEXPRESS01;Database=BudgetTracker;Trusted_Connection=True;TrustServerCertificate=True;";
        optionsBuilder.UseSqlServer(conn);

        return new AppDbContext(optionsBuilder.Options);
    }
}
