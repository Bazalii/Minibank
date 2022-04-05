using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MiniBank.Core.Domains.BankAccounts.Repositories;
using MiniBank.Core.Domains.Users.Repositories;
using ValidationException = MiniBank.Core.Exceptions.ValidationException;

namespace MiniBank.Core.Domains.Users.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        private readonly IBankAccountRepository _bankAccountRepository;

        private readonly IUnitOfWork _unitOfWork;

        private readonly IValidator<UserCreationModel> _userValidator;

        public UserService(IUserRepository userRepository, IBankAccountRepository bankAccountRepository,
            IUnitOfWork unitOfWork, IValidator<UserCreationModel> userValidator)
        {
            _userRepository = userRepository;
            _bankAccountRepository = bankAccountRepository;
            _unitOfWork = unitOfWork;
            _userValidator = userValidator;
        }

        public async Task Add(UserCreationModel model, CancellationToken cancellationToken)
        {
            await _userValidator.ValidateAndThrowAsync(model, cancellationToken);

            await _userRepository.Add(new User
            {
                Id = Guid.NewGuid(),
                Login = model.Login,
                Email = model.Email
            }, cancellationToken);

            await _unitOfWork.SaveChanges(cancellationToken);
        }

        public Task<User> GetById(Guid id, CancellationToken cancellationToken)
        {
            return _userRepository.GetById(id, cancellationToken);
        }

        public Task<IEnumerable<User>> GetAll(CancellationToken cancellationToken)
        {
            return _userRepository.GetAll(cancellationToken);
        }

        public async Task Update(User user, CancellationToken cancellationToken)
        {
            await _userValidator.ValidateAndThrowAsync(new UserCreationModel
            {
                Login = user.Login,
                Email = user.Email
            }, cancellationToken);
            
            await _userRepository.Update(user, cancellationToken);
            
            await _unitOfWork.SaveChanges(cancellationToken);
        }

        public async Task DeleteById(Guid id, CancellationToken cancellationToken)
        {
            var check = await _bankAccountRepository.ExistsForUser(id, cancellationToken);

            if (check)
            {
                throw new ValidationException($"User with id: {id} has connected accounts!");
            }

            await _userRepository.DeleteById(id, cancellationToken);
            await _unitOfWork.SaveChanges(cancellationToken);
        }
    }
}