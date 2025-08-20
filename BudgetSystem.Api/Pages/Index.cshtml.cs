using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BudgetSystem.Api.Pages;

public class IndexModel : PageModel
{
    private readonly ApiClient _api;

    public List<ApiClient.AccountVm> Accounts { get; private set; } = new();

    public IndexModel(ApiClient api)
    {
        _api = api;
    }

    public async Task OnGetAsync()
    {
        Accounts = await _api.GetAccountsAsync();
    }
}
