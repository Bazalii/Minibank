using FluentValidation;
using MiniBank.Core.Domains.Users.Repositories;

namespace MiniBank.Core.Domains.BankAccounts.Validators
{
    public class BankAccountValidator : AbstractValidator<BankAccount>
    {
        public BankAccountValidator(IUserRepository userRepository)
        {
            RuleFor(x => x.AmountOfMoney).GreaterThan(0).WithMessage("should be greater than 0!");
            RuleFor(x => x.UserId).MustAsync(async (userId, cancellationToken) =>
            {
                var userExists = await userRepository.Exists(userId, cancellationToken);
                return userExists;
            }).WithMessage("user with entered Id doesn't exist!");
        }
    }
}