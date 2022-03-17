using System;
using System.Collections.Generic;
using MiniBank.Core.Domains.BankAccounts.Repositories;
using MiniBank.Core.Domains.Users.Repositories;
using MiniBank.Core.Exceptions;

namespace MiniBank.Core.Domains.Users.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        private readonly IBankAccountRepository _bankAccountRepository;

        public UserService(IUserRepository userRepository, IBankAccountRepository bankAccountRepository)
        {
            _userRepository = userRepository;
            _bankAccountRepository = bankAccountRepository;
        }

        public void AddUser(User user)
        {
            _userRepository.Add(user);
        }

        public User GetUserById(Guid id)
        {
            return _userRepository.GetUserById(id);
        }

        public IEnumerable<User> GetAll()
        {
            return _userRepository.GetAll();
        }

        public void UpdateUser(User user)
        {
            _userRepository.Update(user);
        }

        public void DeleteUserById(Guid id)
        {
            if (_bankAccountRepository.CheckIfUserHasConnectedAccounts(id))
            {
                throw new ValidationException($"User with id: {id} has connected accounts!");
            }
            
            _userRepository.DeleteUserById(id);
        }
    }
}