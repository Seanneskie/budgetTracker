using BudgetSystem.Client;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BudgetSystem.Web.Pages.Transactions;

public class IndexModel(ApiClient api) : PageModel
{
    public List<ApiClient.TransactionVm> Transactions { get; private set; } = new();

    public async Task OnGet()
    {
        Transactions = await api.GetTransactionsAsync();
    }
}
