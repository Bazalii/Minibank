using System.Threading.Tasks;
using FluentValidation;
using MiniBank.Core.Domains.Users.Repositories;

namespace MiniBank.Core.Domains.Users.Validators
{
    public class UserValidator : AbstractValidator<UserCreationModel>
    {
        public UserValidator(IUserRepository userRepository)
        {
            RuleFor(x => x.Login).NotEmpty().WithMessage("cannot be empty!");
            RuleFor(x => x.Login.Length).LessThanOrEqualTo(20)
                .WithMessage("cannot contain more than 20 symbols!");
            RuleFor(x => x.Login).MustAsync(async (login, _) =>
                {
                    var exists = await userRepository.IsLoginExists(login);
                    return !exists;
                })
                .WithMessage("should be unique!");

            RuleFor(x => x.Email).NotEmpty().WithMessage("cannot be empty!");
            RuleFor(x => x.Email).MustAsync((email, _) => Task.FromResult(email.Contains('@')))
                .WithMessage("should contain @!");
        }
    }
}