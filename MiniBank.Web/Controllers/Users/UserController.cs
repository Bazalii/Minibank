using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MiniBank.Core.Domains.Users;
using MiniBank.Core.Domains.Users.Services;
using MiniBank.Web.Controllers.Users.Dto;

namespace MiniBank.Web.Controllers.Users
{
    [ApiController]
    [Route("/user")]
    public class UserController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public void Create(UserCreationRequest model)
        {
            _userService.AddUser(new User
            {
                Id = Guid.NewGuid(),
                Email = model.Email,
                Login = model.Login
            });
        }

        [HttpGet("{id:guid}")]
        public UserResponse Get(Guid id)
        {
            var model = _userService.GetUserById(id);
            return new UserResponse
            {
                Id = model.Id,
                Email = model.Email,
                Login = model.Login
            };
        }

        [HttpGet]
        public IEnumerable<UserResponse> GetAll()
        {
            return _userService.GetAll().Select(user => new UserResponse
            {
                Id = user.Id,
                Login = user.Login,
                Email = user.Email
            });
        }

        [HttpPut]
        public void Update(Guid id, UserUpdateRequest model)
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