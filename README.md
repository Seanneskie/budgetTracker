# Budget System (ASP.NET Core + SQL Server)

Layered API: **Domain / Application / Infrastructure / Api**  
EF Core migrations, RFC7807 errors, idempotency keys, and minimal endpoints.

## Run
```bash
dotnet user-secrets set --project BudgetSystem.Api "ConnectionStrings:DefaultConnection" "Server=localhost\\SQLEXPRESS01;Database=BudgetTracker;User Id=...;Password=...;TrustServerCertificate=True;"
dotnet ef database update -p BudgetSystem.Infrastructure -s BudgetSystem.Api
dotnet run --project BudgetSystem.Api
