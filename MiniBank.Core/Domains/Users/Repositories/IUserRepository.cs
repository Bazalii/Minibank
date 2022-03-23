using System;
using System.Collections.Generic;

namespace MiniBank.Core.Domains.Users.Repositories
{
    public interface IUserRepository
    {
        void Add(User user);

        User GetById(Guid id);

        IEnumerable<User> GetAll();

        void Update(User user);

        void DeleteById(Guid id);

        bool Exists(Guid id);
    }
}