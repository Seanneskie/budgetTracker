using BudgetSystem.Api.Endpoints;                 // endpoint modules (Map*Endpoints)
using BudgetSystem.Api.Middleware;               // IdempotencyMiddleware
using BudgetSystem.Infrastructure.Persistence;   // AppDbContext
using BudgetSystem.Application.Validation;       // validators
using FluentValidation;                          // FluentValidation services
using FluentValidation.AspNetCore;               // automatic validation integration
using Microsoft.AspNetCore.Diagnostics;          // exception handler feature
using Microsoft.EntityFrameworkCore;

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

builder.Services.AddCors(o => o.AddDefaultPolicy(p =>
    p.AllowAnyHeader().AllowAnyMethod().WithOrigins(
        "https://localhost:7243", // Kestrel HTTPS port of the Razor app (shown when you run it)
        "http://localhost:5243"   // Kestrel HTTP port of the Razor app
    )));
    
builder.Services.AddRazorPages();

// Typed HttpClient for the API
builder.Services.AddHttpClient<ApiClient>(client =>
{
    var baseUrl = builder.Configuration["Api:BaseUrl"] 
                  ?? throw new InvalidOperationException("Api:BaseUrl missing");
    client.BaseAddress = new Uri(baseUrl);
});


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
            KeyNotFoundException => (StatusCodes.Status404NotFound, "Resource not found"),
            DbUpdateException => (StatusCodes.Status409Conflict, "Database update conflict"),
            ArgumentException or
            InvalidOperationException => (StatusCodes.Status400BadRequest, "Bad request"),
            _ => (StatusCodes.Status500InternalServerError, "Server error")
        };

        await Results.Problem(
            type: $"https://httpstatuses.com/{status}",
            title: title,
            detail: ex?.Message,
            statusCode: status
        ).ExecuteAsync(context);
    });
});

// CORS
app.UseCors();
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
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();
app.Run();


// Simple API client wrapper
public class ApiClient
{
    private readonly HttpClient _http;
    public ApiClient(HttpClient http) => _http = http;

    // Accounts
    public async Task<List<AccountVm>> GetAccountsAsync()
        => await _http.GetFromJsonAsync<List<AccountVm>>("/api/v1/accounts") ?? new();

    public async Task<int> CreateAccountAsync(AccountCreateDto dto)
    {
        var resp = await _http.PostAsJsonAsync("/api/v1/accounts", dto);
        if (!resp.IsSuccessStatusCode)
            throw new HttpRequestException($"Create failed: {(int)resp.StatusCode} {resp.ReasonPhrase}");
        var payload = await resp.Content.ReadFromJsonAsync<CreatedId>();
        return payload?.Id ?? 0;
    }

    private record CreatedId(int Id);
    public record AccountVm(int Id, string Name, decimal StartingBalance, string Currency, DateTime CreatedUtc, DateTime? UpdatedUtc);
    public record AccountCreateDto(string Name, decimal StartingBalance, string Currency = "PHP");
}