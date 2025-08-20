using BudgetSystem.Api.Endpoints;                 // endpoint modules (Map*Endpoints)
using BudgetSystem.Api.Middleware;               // IdempotencyMiddleware
using BudgetSystem.Infrastructure.Persistence;   // AppDbContext
using BudgetSystem.Application.Validation;       // validators
using FluentValidation;                          // FluentValidation services
using Microsoft.AspNetCore.Diagnostics;          // exception handler feature
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using BudgetSystem.Application.Validation;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// RFC 7807 ProblemDetails
builder.Services.AddProblemDetails();

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<TransactionCreateValidator>();

// EF Core
var conn = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(o => o.UseSqlServer(conn));

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<AccountCreateValidator>();
builder.Services.AddFluentValidationAutoValidation();

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Global exception handler -> ProblemDetails
app.UseExceptionHandler(errApp =>
{
    errApp.Run(async context =>
    {
        var ex = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        var (status, title) = ex switch
        {
            KeyNotFoundException         => (StatusCodes.Status404NotFound, "Resource not found"),
            DbUpdateException            => (StatusCodes.Status409Conflict,  "Database update conflict"),
            ArgumentException or
            InvalidOperationException    => (StatusCodes.Status400BadRequest, "Bad request"),
            _                            => (StatusCodes.Status500InternalServerError, "Server error")
        };

        await Results.Problem(
            type: $"https://httpstatuses.com/{status}",
            title: title,
            detail: ex?.Message,
            statusCode: status
        ).ExecuteAsync(context);
    });
});

// Idempotency for POST/PUT/PATCH/DELETE (expects Idempotency-Key header)
app.UseMiddleware<IdempotencyMiddleware>();

// Map endpoints (modules)
app.MapMetaEndpoints();
app.MapAccountEndpoints();
app.MapCategoryEndpoints();
app.MapTransactionEndpoints();
app.MapBudgetEndpoints();

// Auto-apply migrations (dev convenience)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}

app.Run();
