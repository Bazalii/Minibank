using System;

namespace MiniBank.Core.Domains.Users.Repositories
{
    public interface IUserRepository
    {
        void Add(User user);

        public User GetUserById(Guid id);

        public void Update(User user);

        public void DeleteUserById(Guid id);

        public bool CheckByIdIfUserExists(Guid id);
    }
}