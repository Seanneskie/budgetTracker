using BudgetSystem.Application.DTOs;
using BudgetSystem.Domain.Enums;
using FluentValidation;

namespace BudgetSystem.Application.Validation;

public class TransactionUpdateValidator : AbstractValidator<TransactionUpdateDto>
{
    public TransactionUpdateValidator()
    {
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.Type).IsInEnum();

        // Income/Expense
        When(x => x.Type == TransactionType.Income || x.Type == TransactionType.Expense, () =>
        {
            RuleFor(x => x.AccountId).GreaterThan(0);
        });

        // Transfer
        When(x => x.Type == TransactionType.Transfer, () =>
        {
            RuleFor(x => x.FromAccountId).NotNull().WithMessage("FromAccountId is required for transfers.");
            RuleFor(x => x.ToAccountId).NotNull().WithMessage("ToAccountId is required for transfers.");
            RuleFor(x => x).Must(x => x.FromAccountId != x.ToAccountId)
                .WithMessage("FromAccountId and ToAccountId must be different.");
        });
    }
}
