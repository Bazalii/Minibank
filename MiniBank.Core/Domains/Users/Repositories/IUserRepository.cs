using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniBank.Core.Domains.Users.Repositories
{
    public interface IUserRepository
    {
        Task Add(User user);

        Task<User> GetById(Guid id);

        Task<IEnumerable<User>> GetAll();

        Task Update(User user);

        Task DeleteById(Guid id);

        Task<bool> Exists(Guid id);

        Task<bool> IsLoginExists(string login);
    }
}