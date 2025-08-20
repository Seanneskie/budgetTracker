using System.Net.Http.Json;

namespace BudgetSystem.Client;

/// <summary>
/// Thin wrapper over <see cref="HttpClient"/> used by the razor pages to talk to the backend API.
/// </summary>
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
