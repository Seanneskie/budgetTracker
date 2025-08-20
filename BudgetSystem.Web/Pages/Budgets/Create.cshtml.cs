using BudgetSystem.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BudgetSystem.Web.Pages.Budgets;

public class CreateModel : PageModel
{
    private readonly ApiClient _api;

    [BindProperty]
    public ApiClient.BudgetCreateDto Form { get; set; } = new(0, 0, 0m, 0, null);

    public SelectList Accounts { get; set; } = default!;
    public SelectList Categories { get; set; } = default!;

    public CreateModel(ApiClient api)
    {
        _api = api;
    }

    public async Task OnGetAsync()
    {
        await LoadLookups();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadLookups();
            return Page();
        }

        await _api.CreateBudgetAsync(Form);
        return RedirectToPage("Index");
    }

    private async Task LoadLookups()
    {
        var accounts = await _api.GetAccountsAsync();
        Accounts = new SelectList(accounts, nameof(ApiClient.AccountVm.Id), nameof(ApiClient.AccountVm.Name));

        var categories = await _api.GetCategoriesAsync();
        Categories = new SelectList(categories, nameof(ApiClient.CategoryVm.Id), nameof(ApiClient.CategoryVm.Name));
    }
}

