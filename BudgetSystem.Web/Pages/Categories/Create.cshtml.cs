using BudgetSystem.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BudgetSystem.Web.Pages.Categories;

public class CreateModel : PageModel
{
    private readonly ApiClient _api;

    [BindProperty]
    public ApiClient.CategoryCreateDto Form { get; set; } = new("", ApiClient.TransactionType.Expense, null);

    public CreateModel(ApiClient api)
    {
        _api = api;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        await _api.CreateCategoryAsync(Form);
        return RedirectToPage("Index");
    }
}
