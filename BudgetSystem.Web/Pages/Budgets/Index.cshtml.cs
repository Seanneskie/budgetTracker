using BudgetSystem.Client;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BudgetSystem.Web.Pages.Budgets;

public class IndexModel(ApiClient api) : PageModel
{
    public List<ApiClient.BudgetVm> Budgets { get; private set; } = new();

    public async Task OnGet()
    {
        Budgets = await api.GetBudgetsAsync();
    }
}

