using BudgetSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using BudgetSystem.Infrastructure.Idempotency;

namespace BudgetSystem.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<Budget> Budgets => Set<Budget>();
    public DbSet<IdempotencyRecord> IdempotencyRecords => Set<IdempotencyRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Money precision
        modelBuilder.Entity<Account>()
            .Property(p => p.StartingBalance)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Transaction>()
            .Property(p => p.Amount)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Budget>()
            .Property(p => p.LimitAmount)
            .HasColumnType("decimal(18,2)");

        // Required + max lengths
        modelBuilder.Entity<Account>()
            .Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<Account>()
            .Property(p => p.Currency)
            .IsRequired()
            .HasMaxLength(3);

        modelBuilder.Entity<Category>()
            .Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        // Unique category name per account (or global if AccountId is null)
        modelBuilder.Entity<Category>()
            .HasIndex(c => new { c.AccountId, c.Name })
            .IsUnique();

        // Relationships
        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.Account)
            .WithMany(a => a.Transactions)
            .HasForeignKey(t => t.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.Category)
            .WithMany()
            .HasForeignKey(t => t.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Budget>()
            .HasOne(b => b.Account)
            .WithMany()
            .HasForeignKey(b => b.AccountId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Budget>()
            .HasOne(b => b.Category)
            .WithMany()
            .HasForeignKey(b => b.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);
            
        modelBuilder.Entity<IdempotencyRecord>()
        .HasIndex(i => new { i.Key, i.Path })
        .IsUnique();

        base.OnModelCreating(modelBuilder);
    }
}
