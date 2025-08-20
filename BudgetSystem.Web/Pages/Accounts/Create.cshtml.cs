using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BudgetSystem.Web.Pages.Accounts;

public class CreateModel(ApiClient api) : PageModel
{
    [BindProperty] public CreateAccountForm Form { get; set; } = new();

    public void OnGet() {}

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var id = await api.CreateAccountAsync(
            new ApiClient.AccountCreateDto(Form.Name, Form.StartingBalance, Form.Currency ?? "PHP"));

        TempData["Message"] = $"Created account #{id}";
        return RedirectToPage("Index");
    }

    public class CreateAccountForm
    {
        public string Name { get; set; } = string.Empty;
        public decimal StartingBalance { get; set; }
        public string? Currency { get; set; } = "PHP";
    }
}
