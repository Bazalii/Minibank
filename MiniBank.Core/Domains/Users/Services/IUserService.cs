using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniBank.Core.Domains.Users.Services
{
    public interface IUserService
    {
        Task Add(UserCreationModel model);

        Task<User> GetById(Guid id);

        Task<IEnumerable<User>> GetAll();

        Task Update(User user);

        Task DeleteById(Guid id);
    }
}