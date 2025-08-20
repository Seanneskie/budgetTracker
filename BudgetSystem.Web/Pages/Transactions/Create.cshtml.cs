using BudgetSystem.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BudgetSystem.Web.Pages.Transactions;

public class CreateModel : PageModel
{
    private readonly ApiClient _api;

    public List<ApiClient.AccountVm> Accounts { get; private set; } = new();
    public List<ApiClient.CategoryVm> Categories { get; private set; } = new();

    public SelectList AccountOptions => new(Accounts, "Id", "Name");
    public SelectList CategoryOptions => new(Categories, "Id", "Name");

    [BindProperty]
    public ApiClient.TransactionCreateDto Form { get; set; } =
        new(DateTime.UtcNow, 0m, ApiClient.TransactionType.Expense, 0);

    public CreateModel(ApiClient api)
    {
        _api = api;
    }

    public async Task OnGet()
    {
        Accounts = await _api.GetAccountsAsync();
        Categories = await _api.GetCategoriesAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        Accounts = await _api.GetAccountsAsync();
        Categories = await _api.GetCategoriesAsync();

        if (!ModelState.IsValid)
        {
            return Page();
        }

        await _api.CreateTransactionAsync(Form);
        return RedirectToPage("Index");
    }
}
