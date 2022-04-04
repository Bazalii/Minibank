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
        
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUserRepository userRepository, IBankAccountRepository bankAccountRepository,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _bankAccountRepository = bankAccountRepository;
            _unitOfWork = unitOfWork;
        }

        public void Add(UserCreationModel model)
        {
            _userRepository.Add(new User
            {
                Id = Guid.NewGuid(),
                Login = model.Login,
                Email = model.Email
            });
            
            _unitOfWork.SaveChanges();
        }

        public User GetById(Guid id)
        {
            return _userRepository.GetById(id);
        }

        public IEnumerable<User> GetAll()
        {
            return _userRepository.GetAll();
        }

        public void Update(User user)
        {
            _userRepository.Update(user);
            _unitOfWork.SaveChanges();
        }

        public void DeleteById(Guid id)
        {
            if (_bankAccountRepository.ExistsForUser(id))
            {
                throw new ValidationException($"User with id: {id} has connected accounts!");
            }

            _userRepository.DeleteById(id);
            _unitOfWork.SaveChanges();
        }
    }
}