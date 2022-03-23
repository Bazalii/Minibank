using System;
using System.Collections.Generic;

namespace MiniBank.Core.Domains.Users.Services
{
    public interface IUserService
    {
        void Add(UserCreationModel model);

        User GetById(Guid id);

        IEnumerable<User> GetAll();

        void Update(User user);

        void DeleteById(Guid id);
    }
}