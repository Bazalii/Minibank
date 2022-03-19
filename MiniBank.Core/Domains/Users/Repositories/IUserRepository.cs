using System;
using System.Collections.Generic;

namespace MiniBank.Core.Domains.Users.Repositories
{
    public interface IUserRepository
    {
        void Add(User user);

        public User GetUserById(Guid id);

        public IEnumerable<User> GetAll();

        public void Update(User user);

        public void DeleteUserById(Guid id);

        public int CheckByIdIfUserExists(Guid id);
    }
}