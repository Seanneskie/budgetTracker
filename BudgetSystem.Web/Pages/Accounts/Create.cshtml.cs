using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BudgetSystem.Web.Pages.Accounts;

public class CreateModel : PageModel
{
    private readonly ApiClient _api;

    [BindProperty]
    public ApiClient.AccountCreateDto Form { get; set; } = new("", 0m);

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

        await _api.CreateAccountAsync(Form);
        return RedirectToPage("/Index");
    }
}
