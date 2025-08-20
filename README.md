# Budget System (ASP.NET Core + SQL Server)

Layered API: **Domain / Application / Infrastructure / Api**
EF Core migrations, RFC7807 errors, idempotency keys, and minimal endpoints.

## Run
```bash
dotnet user-secrets set --project BudgetSystem.Api "ConnectionStrings:DefaultConnection" "Server=localhost\\SQLEXPRESS01;Database=BudgetTracker;User Id=...;Password=...;TrustServerCertificate=True;"
dotnet ef database update -p BudgetSystem.Infrastructure -s BudgetSystem.Api
dotnet run --project BudgetSystem.Api
```

## Project structure
- `BudgetSystem.Domain` – Core entities and domain logic.
- `BudgetSystem.Application` – Application services and business rules.
- `BudgetSystem.Infrastructure` – Data access, EF Core implementation, and other infrastructure concerns.
- `BudgetSystem.Api` – ASP.NET Core minimal API exposing the system.
- `BudgetSystem.Client` – Example client for interacting with the API.
- `BudgetSystem.Web` – Web UI frontend.

## Build and test
To compile the solution and run tests:

```bash
dotnet build
dotnet test
```

