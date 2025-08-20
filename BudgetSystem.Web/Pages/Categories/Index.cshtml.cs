using BudgetSystem.Client;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace BudgetSystem.Web.Pages.Categories;

public class IndexModel(ApiClient api) : PageModel
{
    public record CategoryViewModel(int Id, string Name, ApiClient.TransactionType Type, string? AccountName, DateTime CreatedUtc);

    public List<CategoryViewModel> Categories { get; private set; } = new();

    public async Task OnGet()
    {
        var categories = await api.GetCategoriesAsync();
        var accounts = await api.GetAccountsAsync();
        var accountLookup = accounts.ToDictionary(a => a.Id, a => a.Name);

        Categories = categories.Select(c =>
            new CategoryViewModel(
                c.Id,
                c.Name,
                c.Type,
                c.AccountId.HasValue && accountLookup.TryGetValue(c.AccountId.Value, out var name) ? name : null,
                c.CreatedUtc)).ToList();
    }
}
