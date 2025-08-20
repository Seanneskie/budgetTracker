using BudgetSystem.Application.DTOs;
using FluentValidation;

namespace BudgetSystem.Application.Validation;

public class AccountCreateValidator : AbstractValidator<AccountCreateDto>
{
    public AccountCreateValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Currency).NotEmpty().Length(3);
        RuleFor(x => x.StartingBalance).GreaterThanOrEqualTo(0);
    }
}

public class AccountUpdateValidator : AbstractValidator<AccountUpdateDto>
{
    public AccountUpdateValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Currency).NotEmpty().Length(3);
    }
}
