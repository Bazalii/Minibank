using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using MiniBank.Core.Domains.BankAccounts.Repositories;
using MiniBank.Core.Domains.Users;
using MiniBank.Core.Domains.Users.Repositories;
using MiniBank.Core.Domains.Users.Services;
using MiniBank.Core.Domains.Users.Services.Implementations;
using MiniBank.Data.Exceptions;
using Moq;
using Xunit;
using ValidationException = MiniBank.Core.Exceptions.ValidationException;

namespace MiniBank.Core.Tests;

public class UserServiceTests
{
    private readonly IUserService _userService;

    private readonly Mock<IUserRepository> _mockUserRepository;

    private readonly Mock<IBankAccountRepository> _mockBankAccountRepository;

    private readonly Mock<IUnitOfWork> _mockUnitOfWork;

    private readonly Mock<IValidator<User>> _mockUserValidator;

    public UserServiceTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockBankAccountRepository = new Mock<IBankAccountRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockUserValidator = new Mock<IValidator<User>>();

        _userService = new UserService(_mockUserRepository.Object, _mockBankAccountRepository.Object,
            _mockUnitOfWork.Object, _mockUserValidator.Object);
    }

    [Fact]
    public async Task Add_SuccessPath_AccountIsValidatedAddInUserRepositoryAndSaveChangesInUnitOfWorkAreCalled()
    {
        // ARRANGE
        const string login = "Example";

        var userCreationModel = new UserCreationModel
        {
            Login = login
        };

        // ACT
        await _userService.Add(userCreationModel, default);

        // ASSERT
        _mockUserValidator
            .Verify(
                validator => validator.ValidateAsync(It.IsAny<ValidationContext<User>>(), default),
                Times.Once());
        _mockUserRepository
            .Verify(repository => repository.Add(It.Is<User>(user => user.Login == login), default),
                Times.Once());
        _mockUnitOfWork
            .Verify(unitOfWork => unitOfWork.SaveChanges(default), Times.Once());
    }

    [Fact]
    public async Task GetById_UserNotExists_ThrowException()
    {
        // ARRANGE
        var userId = Guid.NewGuid();

        _mockUserRepository
            .Setup(repository => repository.GetById(userId, default))
            .ThrowsAsync(new ObjectNotFoundException($"User with id: {userId} is not found!"));

        // ACT
        var exception = await Assert.ThrowsAsync<ObjectNotFoundException>(async () =>
            await _userService.GetById(userId, default));

        // ASSERT
        Assert.Equal($"User with id: {userId} is not found!", exception.Message);
    }

    [Fact]
    public async Task GetById_SuccessPath_GetByIdInUserRepositoryIsCalledReturnsCorrespondingUser()
    {
        // ARRANGE
        var userId = Guid.NewGuid();

        var user = new User
        {
            Id = userId
        };

        _mockUserRepository
            .Setup(repository => repository.GetById(userId, default))
            .ReturnsAsync(user);

        // ACT
        var result = await _userService.GetById(userId, default);

        // ASSERT
        _mockUserRepository
            .Verify(repository => repository.GetById(userId, default), Times.Once());

        Assert.Equal(user, result);
    }

    [Theory]
    [MemberData(nameof(GetDataForGetAllTest))]
    public async Task GetAll_SuccessPath_GetAllInUserRepositoryIsCalledReturnsIEnumerableOfUsers(
        params User[] expectedResult)
    {
        // ARRANGE
        _mockUserRepository
            .Setup(repository => repository.GetAll(default))
            .ReturnsAsync(expectedResult);

        // ACT
        var users = await _userService.GetAll(default);

        // ASSERT
        _mockUserRepository
            .Verify(repository => repository.GetAll(default), Times.Once());

        Assert.Equal(expectedResult, users);
    }

    [Fact]
    public async Task Update_UserNotExists_ThrowException()
    {
        // ARRANGE
        var userId = Guid.NewGuid();

        var user = new User
        {
            Id = userId
        };

        _mockUserRepository
            .Setup(repository => repository.Update(user, default))
            .ThrowsAsync(new ObjectNotFoundException($"User with id: {userId} is not found!"));

        // ACT
        var exception = await Assert.ThrowsAsync<ObjectNotFoundException>(async () =>
            await _userService.Update(user, default));

        // ASSERT
        Assert.Equal($"User with id: {userId} is not found!", exception.Message);
    }

    [Fact]
    public async Task Update_SuccessPath_UserIsValidatedUpdateInUserRepositoryAndSaveChangesInUnitOfWorkAreCalled()
    {
        // ARRANGE
        var userId = Guid.NewGuid();

        var user = new User
        {
            Id = userId
        };

        // ACT
        await _userService.Update(user, default);

        // ASSERT
        _mockUserValidator
            .Verify(validator => validator.ValidateAsync(It.IsAny<ValidationContext<User>>(), default),
                Times.Once());
        _mockUserRepository
            .Verify(repository => repository.Update(user, default), Times.Once());
        _mockUnitOfWork
            .Verify(unitOfWork => unitOfWork.SaveChanges(default), Times.Once());
    }

    [Fact]
    public async Task DeleteById_UserNotExists_ThrowException()
    {
        // ARRANGE
        var userId = Guid.NewGuid();

        _mockUserRepository
            .Setup(repository => repository.DeleteById(userId, default))
            .ThrowsAsync(new ObjectNotFoundException($"User with id: {userId} is not found!"));

        // ACT
        var exception = await Assert.ThrowsAsync<ObjectNotFoundException>(async () =>
            await _userService.DeleteById(userId, default));

        // ASSERT
        Assert.Equal($"User with id: {userId} is not found!", exception.Message);
    }

    [Fact]
    public async Task DeleteById_UserHasConnectedAccounts_ThrowException()
    {
        // ARRANGE
        var userId = Guid.NewGuid();

        _mockBankAccountRepository
            .Setup(repository => repository.ExistsForUser(userId, default))
            .ReturnsAsync(true);

        // ACT
        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
            await _userService.DeleteById(userId, default));

        // ASSERT
        Assert.Equal($"User with id: {userId} has connected accounts!", exception.Message);
    }

    [Fact]
    public async Task DeleteById_SuccessPath_DeleteByIdInUserRepositoryAndSaveChangesInUnitOfWorkAreCalled()
    {
        // ARRANGE
        var userId = Guid.NewGuid();

        _mockBankAccountRepository
            .Setup(repository => repository.ExistsForUser(userId, default))
            .ReturnsAsync(false);

        // ACT
        await _userService.DeleteById(userId, default);

        // ASSERT
        _mockUserRepository
            .Verify(repository => repository.DeleteById(userId, default), Times.Once);
        _mockUnitOfWork
            .Verify(unitOfWork => unitOfWork.SaveChanges(default), Times.Once);
    }

    private static IEnumerable<object[]> GetDataForGetAllTest()
    {
        var data = new List<object[]>
        {
            Array.Empty<object>(),
            new object[] { new User(), new User() }
        };

        return data;
    }
}