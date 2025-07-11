﻿using FluentAssertions;
using Moq;
using UserService.Application.Interfaces;
using UserService.Application.Users.Commands.RegisterUser;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces;

namespace Tests.UserService;

public class RegisterUserHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly RegisterUserHandler _handler;

    public RegisterUserHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _handler = new RegisterUserHandler(_userRepositoryMock.Object, _passwordHasherMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldRegisterUser_WhenEmailIsUnique()
    {
        var command = new RegisterUserCommand("test@test.com", "password", "John Doe");
        _userRepositoryMock
            .Setup(repo => repo.GetByEmailAsync(command.Email))
            .ReturnsAsync(User.Empty);
        _passwordHasherMock
            .Setup(hasher => hasher.HashPassword(It.IsAny<string>()))
            .ReturnsAsync("123");
        
        var result = await _handler.Handle(command, new CancellationToken());
        
        result.IsSuccess.Should().BeTrue();
        _userRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Once);
        _passwordHasherMock.Verify(hasher => hasher.HashPassword(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldNotRegisterUser_WhenEmailIsNotUnique()
    {
        var command = new RegisterUserCommand("test@test.com", "password", "John Doe");
        _userRepositoryMock
            .Setup(repo => repo.GetByEmailAsync(command.Email))
            .ReturnsAsync(new User("123@123.com", "123", "John Doe"));
        
        var result = await _handler.Handle(command, new CancellationToken());
        
        result.IsSuccess.Should().BeFalse();
        _userRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Never);
        _passwordHasherMock.Verify(hasher => hasher.HashPassword(It.IsAny<string>()), Times.Never);
    }
}