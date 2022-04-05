using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MiniBank.Core.Domains.Users.Services
{
    public interface IUserService
    {
        Task Add(UserCreationModel model, CancellationToken cancellationToken);

        Task<User> GetById(Guid id, CancellationToken cancellationToken);

        Task<IEnumerable<User>> GetAll(CancellationToken cancellationToken);

        Task Update(User user, CancellationToken cancellationToken);

        Task DeleteById(Guid id, CancellationToken cancellationToken);
    }
}