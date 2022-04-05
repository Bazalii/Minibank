using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MiniBank.Core.Domains.Users;
using MiniBank.Core.Domains.Users.Repositories;
using MiniBank.Data.Exceptions;

namespace MiniBank.Data.Users.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MiniBankContext _context;

        public UserRepository(MiniBankContext context)
        {
            _context = context;
        }

        public Task Add(User user, CancellationToken cancellationToken)
        {
            return _context.Users.AddAsync(new UserDbModel
            {
                Id = user.Id,
                Login = user.Login,
                Email = user.Email
            }, cancellationToken).AsTask();
        }

        public async Task<User> GetById(Guid id, CancellationToken cancellationToken)
        {
            var dbModel = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(currentUser => currentUser.Id == id, cancellationToken);

            if (dbModel == null)
            {
                throw new ObjectNotFoundException($"User with id: {id} is not found!");
            }

            return new User
            {
                Id = dbModel.Id,
                Login = dbModel.Login,
                Email = dbModel.Email
            };
        }

        public async Task<IEnumerable<User>> GetAll(CancellationToken cancellationToken)
        {
            return await _context.Users
                .AsNoTracking()
                .Select(user => new User
                {
                    Id = user.Id,
                    Login = user.Login,
                    Email = user.Email
                }).ToListAsync(cancellationToken);
        }

        public async Task Update(User user, CancellationToken cancellationToken)
        {
            var dbModel =
                await _context.Users.FirstOrDefaultAsync(currentUser => currentUser.Id == user.Id, cancellationToken);

            if (dbModel == null)
            {
                throw new ObjectNotFoundException($"User with id: {user.Id} is not found!");
            }

            dbModel.Login = user.Login;
            dbModel.Email = user.Email;
        }

        public async Task DeleteById(Guid id, CancellationToken cancellationToken)
        {
            var dbModel = await FindUser(id, cancellationToken);
            _context.Users.Remove(dbModel);
        }

        public async Task<bool> Exists(Guid id, CancellationToken cancellationToken)
        {
            var dbModel = await FindUser(id, cancellationToken);
            return dbModel != null;
        }

        public async Task<bool> IsLoginExists(string login, CancellationToken cancellationToken)
        {
            var dbModel = await _context.Users.FirstOrDefaultAsync(user => user.Login == login, cancellationToken);
            return dbModel != null;
        }

        private Task<UserDbModel> FindUser(Guid id, CancellationToken cancellationToken)
        {
            return _context.Users.FirstOrDefaultAsync(user => user.Id == id, cancellationToken);
        }
    }
}