using System;

namespace MiniBank.Core.Domains.Users.Services
{
    public interface IUserService
    {
        void AddUser(User user);

        public User GetUserById(Guid id);

        public void UpdateUser(User user);

        public void DeleteUserById(Guid id);
    }
}