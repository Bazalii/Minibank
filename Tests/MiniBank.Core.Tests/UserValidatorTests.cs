using System;
using System.Threading.Tasks;
using FluentValidation;
using MiniBank.Core.Domains.Users;
using MiniBank.Core.Domains.Users.Repositories;
using MiniBank.Core.Domains.Users.Validators;
using Moq;
using Xunit;

namespace MiniBank.Core.Tests;

public class UserValidatorTests
{
    private readonly AbstractValidator<User> _userValidator;

    private readonly Mock<IUserRepository> _mockUserRepository;

    public UserValidatorTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();

        _userValidator = new UserValidator(_mockUserRepository.Object);
    }

    [Fact]
    public async Task ValidateAndTrowAsync_UserWithEmptyLogin_ThrowException()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Login = string.Empty,
            Email = "example@example.com"
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() =>
            _userValidator.ValidateAndThrowAsync(user));

        Assert.Contains("Login: cannot be empty!", exception.Message);
    }

    [Fact]
    public async Task ValidateAndTrowAsync_UserWithLoginThatContainsMoreThan20Symbols_ThrowException()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Login = "ExampleExampleExampleExampleExample",
            Email = "example@example.com"
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() =>
            _userValidator.ValidateAndThrowAsync(user));

        Assert.Contains("Login.Length: cannot contain more than 20 symbols!", exception.Message);
    }

    [Fact]
    public async Task ValidateAndTrowAsync_UserWithExistingLogin_ThrowException()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Login = "Example",
            Email = "example@example.com"
        };

        _mockUserRepository
            .Setup(repository => repository.IsLoginExists("Example", default))
            .ReturnsAsync(true);

        var exception = await Assert.ThrowsAsync<ValidationException>(() =>
            _userValidator.ValidateAndThrowAsync(user));

        Assert.Contains("Login: should be unique!", exception.Message);
    }

    [Fact]
    public async Task ValidateAndTrowAsync_UserWithEmptyEmail_ThrowException()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Login = "Example",
            Email = string.Empty
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() =>
            _userValidator.ValidateAndThrowAsync(user));

        Assert.Contains("Email: cannot be empty!", exception.Message);
    }

    [Fact]
    public async Task ValidateAndTrowAsync_UserWithEmailThatNotContainsDog_ThrowException()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Login = "Example",
            Email = "example.com"
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() =>
            _userValidator.ValidateAndThrowAsync(user));

        Assert.Contains("Email: should contain @!", exception.Message);
    }

    [Fact]
    public async Task ValidateAndTrowAsync_SuccessPath_UserIsValidated()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Login = "Example",
            Email = "example@example.com"
        };

        await _userValidator.ValidateAndThrowAsync(user);

        _mockUserRepository
            .Verify(
                repository => repository.IsLoginExists(user.Login, default),
                Times.Once());
    }
}