using BudgetSystem.Client;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BudgetSystem.Web.Pages.Categories;

public class IndexModel(ApiClient api) : PageModel
{
    public List<ApiClient.CategoryVm> Categories { get; private set; } = new();

    public async Task OnGet()
    {
        Categories = await api.GetCategoriesAsync();
    }
}
