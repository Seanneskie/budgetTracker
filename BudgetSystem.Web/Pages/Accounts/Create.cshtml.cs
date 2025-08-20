using BudgetSystem.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BudgetSystem.Web.Pages.Accounts;

public class CreateModel : PageModel
{
    private readonly ApiClient _api;

    [BindProperty]
    public AccountFormModel Form { get; set; } = new();

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

        var dto = new ApiClient.AccountCreateDto(Form.Name, Form.StartingBalance, Form.Currency);
        await _api.CreateAccountAsync(dto);
        return RedirectToPage("/Index");
    }
}
