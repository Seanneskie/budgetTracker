using BudgetSystem.Application.DTOs;
using FluentValidation;

namespace BudgetSystem.Application.Validation;

public class BudgetCreateValidator : AbstractValidator<BudgetCreateDto>
{
    public BudgetCreateValidator()
    {
        RuleFor(x => x.Year).GreaterThan(0);
        RuleFor(x => x.Month).InclusiveBetween(1, 12);
        RuleFor(x => x.LimitAmount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.AccountId).GreaterThan(0);
        RuleFor(x => x.CategoryId).GreaterThan(0).When(x => x.CategoryId.HasValue);
    }
}

public class BudgetUpdateValidator : AbstractValidator<BudgetUpdateDto>
{
    public BudgetUpdateValidator()
    {
        RuleFor(x => x.LimitAmount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.CategoryId).GreaterThan(0).When(x => x.CategoryId.HasValue);
    }
}
