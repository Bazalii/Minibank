using FluentValidation;
using MiniBank.Core.Domains.Users.Repositories;

namespace MiniBank.Core.Domains.BankAccounts.Validators
{
    public class BankAccountValidator : AbstractValidator<BankAccountCreationModel>
    {
        public BankAccountValidator(IUserRepository userRepository)
        {
            RuleFor(x => x.UserId).MustAsync(async (userId, cancellationToken) =>
            {
                var userExists = await userRepository.Exists(userId, cancellationToken);
                return userExists;
            }).WithMessage("user with entered Id doesn't exist!");
        }
    }
}