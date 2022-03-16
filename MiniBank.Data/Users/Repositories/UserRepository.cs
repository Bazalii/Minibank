using System;
using System.Collections.Generic;
using System.Linq;
using MiniBank.Core.Domains.Users;
using MiniBank.Core.Domains.Users.Repositories;
using MiniBank.Data.Exceptions;

namespace MiniBank.Data.Users.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly List<UserDbModel> _users = new ();
        public void Add(User user)
        {
            _users.Add(new UserDbModel
            {
                Id = user.Id,
                Login = user.Login,
                Email = user.Email
            });
        }

        public User GetUserById(Guid id)
        {
            var wantedUser = _users.FirstOrDefault(currentUser => currentUser.Id == id);
            if (wantedUser == null)
            {
                throw new ObjectNotFoundException($"User with id: {id} is not found!");
            }

            return new User
            {
                Id = wantedUser.Id,
                Login = wantedUser.Login,
                Email = wantedUser.Email
            };
        }

        public void Update(User user)
        {
            var wantedUser = _users.FirstOrDefault(currentUser => currentUser.Id == user.Id);
            if (wantedUser == null)
            {
                throw new ObjectNotFoundException($"User with id: {user.Id} is not found!");
            }

            wantedUser.Login = user.Login;
            wantedUser.Email = user.Email;
        }

        public void DeleteUserById(Guid id)
        {
            var wantedUser = _users.FirstOrDefault(currentUser => currentUser.Id == id);
            if (wantedUser == null)
            {
                throw new ObjectNotFoundException($"User with id: {id} is not found!");
            }

            _users.Remove(wantedUser);
        }

        public bool CheckByIdIfUserExists(Guid id)
        {
            return GetUserById(id) != null;
        }
    }
}