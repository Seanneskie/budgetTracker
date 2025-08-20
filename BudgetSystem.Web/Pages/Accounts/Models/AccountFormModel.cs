using System.ComponentModel.DataAnnotations;

namespace BudgetSystem.Web.Pages.Accounts;

public class AccountFormModel
{
    [Display(Name = "Name")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Starting Balance")]
    public decimal StartingBalance { get; set; }

    [Display(Name = "Currency")]
    public string Currency { get; set; } = string.Empty;
}

