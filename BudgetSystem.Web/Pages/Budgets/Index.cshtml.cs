using BudgetSystem.Client;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace BudgetSystem.Web.Pages.Budgets;

public class IndexModel(ApiClient api) : PageModel
{
    public record BudgetViewModel(int Id, int Year, int Month, decimal LimitAmount, string AccountName, string? CategoryName, DateTime CreatedUtc);

    public List<BudgetViewModel> Budgets { get; private set; } = new();

    public async Task OnGet()
    {
        var budgets = await api.GetBudgetsAsync();
        var accounts = await api.GetAccountsAsync();
        var accountLookup = accounts.ToDictionary(a => a.Id, a => a.Name);
        var categories = await api.GetCategoriesAsync();
        var categoryLookup = categories.ToDictionary(c => c.Id, c => c.Name);

        Budgets = budgets.Select(b =>
            new BudgetViewModel(
                b.Id,
                b.Year,
                b.Month,
                b.LimitAmount,
                accountLookup.TryGetValue(b.AccountId, out var accName) ? accName : string.Empty,
                b.CategoryId.HasValue && categoryLookup.TryGetValue(b.CategoryId.Value, out var catName) ? catName : null,
                b.CreatedUtc)).ToList();
    }
}

