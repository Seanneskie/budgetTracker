using BudgetSystem.Client;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BudgetSystem.Web.Pages.Accounts;

public class IndexModel(ApiClient api) : PageModel
{
    public List<ApiClient.AccountVm> Accounts { get; private set; } = new();

    public async Task OnGet()
    {
        Accounts = await api.GetAccountsAsync();
    }
}
