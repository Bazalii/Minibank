using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        public Task Create(UserCreationRequest model, CancellationToken cancellationToken)
        {
            return _userService.Add(new UserCreationModel
            {
                Email = model.Email,
                Login = model.Login
            }, cancellationToken);
        }

        [HttpGet("{id:guid}")]
        public async Task<UserResponse> Get(Guid id, CancellationToken cancellationToken)
        {
            var model = await _userService.GetById(id, cancellationToken);
            return new UserResponse
            {
                Id = model.Id,
                Email = model.Email,
                Login = model.Login
            };
        }

        [HttpGet]
        public async Task<IEnumerable<UserResponse>> GetAll(CancellationToken cancellationToken)
        {
            var users = await _userService.GetAll(cancellationToken);
            return users.Select(user => new UserResponse
            {
                Id = user.Id,
                Login = user.Login,
                Email = user.Email
            });
        }

        [HttpPut]
        public Task Update(Guid id, UserUpdateRequest model, CancellationToken cancellationToken)
        {
            return _userService.Update(new User
            {
                Id = id,
                Email = model.Email,
                Login = model.Login
            }, cancellationToken);
        }

        [HttpDelete]
        public Task Delete(Guid id, CancellationToken cancellationToken)
        {
            return _userService.DeleteById(id, cancellationToken);
        }
    }
}