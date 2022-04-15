using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MiniBank.Core.Domains.Users.Repositories
{
    public interface IUserRepository
    {
        Task Add(User user, CancellationToken cancellationToken);

        Task<User> GetById(Guid id, CancellationToken cancellationToken);

        Task<IEnumerable<User>> GetAll(CancellationToken cancellationToken);

        Task Update(User user, CancellationToken cancellationToken);

        Task DeleteById(Guid id, CancellationToken cancellationToken);

        Task<bool> Exists(Guid id, CancellationToken cancellationToken);

        Task<bool> IsLoginExists(string login, CancellationToken cancellationToken);
    }
}