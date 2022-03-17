using System;
using System.Collections.Generic;

namespace MiniBank.Core.Domains.Users.Services
{
    public interface IUserService
    {
        void AddUser(User user);

        public User GetUserById(Guid id);

        public IEnumerable<User> GetAll();

        public void UpdateUser(User user);

        public void DeleteUserById(Guid id);
    }
}