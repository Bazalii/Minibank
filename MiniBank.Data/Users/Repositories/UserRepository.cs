using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task Add(User user)
        {
            await _context.Users.AddAsync(new UserDbModel
            {
                Id = user.Id,
                Login = user.Login,
                Email = user.Email
            });
        }

        public async Task<User> GetById(Guid id)
        {
            var dbModel = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(currentUser => currentUser.Id == id);
            
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

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users
                .AsNoTracking()
                .Select(user => new User
            {
                Id = user.Id,
                Login = user.Login,
                Email = user.Email
            }).ToListAsync();
        }

        public async Task Update(User user)
        {
            var dbModel = await _context.Users.FirstOrDefaultAsync(currentUser => currentUser.Id == user.Id);
            
            if (dbModel == null)
            {
                throw new ObjectNotFoundException($"User with id: {user.Id} is not found!");
            }

            dbModel.Login = user.Login;
            dbModel.Email = user.Email;
        }

        public async Task DeleteById(Guid id)
        {
            var dbModel = await FindUser(id);
            _context.Users.Remove(dbModel);
        }

        public async Task<bool> Exists(Guid id)
        {
            var dbModel = await FindUser(id);
            return dbModel != null;
        }

        public async Task<bool> IsLoginExists(string login)
        {
            var dbModel = await _context.Users.FirstOrDefaultAsync(user => user.Login == login);
            return dbModel != null;
        }

        private async Task<UserDbModel> FindUser(Guid id)
        {
            var dbModel = await _context.Users.FindAsync(id);
            return dbModel;
        }
    }
}