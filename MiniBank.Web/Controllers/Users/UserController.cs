using System;
using Microsoft.AspNetCore.Mvc;
using MiniBank.Core.Domains.Users;
using MiniBank.Core.Domains.Users.Services;
using MiniBank.Web.Controllers.Users.Dto;

namespace MiniBank.Web.Controllers.Users
{
    [ApiController]
    [Route("[controller]")]
    public class UserController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public void Create(UserRequest user)
        {
            _userService.AddUser(new User
            {
                Id = Guid.NewGuid(),
                Email = user.Email,
                Login = user.Login
            });
        }

        [HttpGet]
        public UserResponse Get(Guid id)
        {
            var wantedUser = _userService.GetUserById(id);
            return new UserResponse
            {
                Id = wantedUser.Id,
                Email = wantedUser.Email,
                Login = wantedUser.Login
            };
        }

        [HttpPut]
        public void Update(Guid id, UserRequest model)
        {
            _userService.UpdateUser(new User
            {
                Id = id,
                Email = model.Email,
                Login = model.Login
            });
        }

        [HttpDelete]
        public void Delete(Guid id)
        {
            _userService.DeleteUserById(id);
        }
    }
}