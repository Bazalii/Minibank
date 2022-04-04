using FluentValidation;
using MiniBank.Core.Domains.Users.Repositories;

namespace MiniBank.Core.Domains.BankAccounts.Validators;

public class BankAccountValidator : AbstractValidator<BankAccountCreationModel>
{
    public BankAccountValidator(IUserRepository userRepository)
    {
        RuleFor(x => x.UserId).MustAsync(async (userId, _) =>
        {
            var userExists = await userRepository.Exists(userId);
            return userExists;
        });
    }
}