using BudgetSystem.Client;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace BudgetSystem.Web.Pages.Transactions;

public class IndexModel(ApiClient api) : PageModel
{
    public record TransactionViewModel(int Id, DateTime Date, decimal Amount, ApiClient.TransactionType Type, string AccountName, string? CategoryName);

    public List<TransactionViewModel> Transactions { get; private set; } = new();

    public async Task OnGet()
    {
        var transactions = await api.GetTransactionsAsync();
        var accounts = await api.GetAccountsAsync();
        var accountLookup = accounts.ToDictionary(a => a.Id, a => a.Name);
        var categories = await api.GetCategoriesAsync();
        var categoryLookup = categories.ToDictionary(c => c.Id, c => c.Name);

        Transactions = transactions.Select(t =>
            new TransactionViewModel(
                t.Id,
                t.Date,
                t.Amount,
                t.Type,
                accountLookup.TryGetValue(t.AccountId, out var accName) ? accName : string.Empty,
                t.CategoryId.HasValue && categoryLookup.TryGetValue(t.CategoryId.Value, out var catName) ? catName : null)).ToList();
    }
}
