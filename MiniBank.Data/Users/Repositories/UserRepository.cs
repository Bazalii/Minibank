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

        public void Add(User user)
        {
            _context.Users.Add(new UserDbModel
            {
                Id = user.Id,
                Login = user.Login,
                Email = user.Email
            });
        }

        public User GetById(Guid id)
        {
            var dbModel = _context.Users
                .AsNoTracking()
                .FirstOrDefault(currentUser => currentUser.Id == id);
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

        public IEnumerable<User> GetAll()
        {
            return _context.Users
                .AsNoTracking()
                .Select(user => new User
            {
                Id = user.Id,
                Login = user.Login,
                Email = user.Email
            });
        }

        public void Update(User user)
        {
            var dbModel = _context.Users.FirstOrDefault(currentUser => currentUser.Id == user.Id);
            if (dbModel == null)
            {
                throw new ObjectNotFoundException($"User with id: {user.Id} is not found!");
            }

            dbModel.Login = user.Login;
            dbModel.Email = user.Email;
        }

        public void DeleteById(Guid id)
        {
            _context.Users.Remove(FindUser(id));
        }

        public bool Exists(Guid id)
        {
            var dbModel = FindUser(id);
            return dbModel != null;
        }

        private UserDbModel FindUser(Guid id)
        {
            var dbModel = _context.Users.Find(id);
            return dbModel;
        }
    }
}