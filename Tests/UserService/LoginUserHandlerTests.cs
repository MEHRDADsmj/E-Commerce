using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using UserService.Application.Interfaces;
using UserService.Application.Users.Commands.LoginUser;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces;

namespace Tests.UserService;

public class LoginUserHandlerTests
{
    private readonly Mock<IUserRepository> _userRepository;
    private readonly Mock<IPasswordHasher> _passwordHasher;
    private readonly Mock<ITokenGenerator> _tokenGenerator;
    private readonly Mock<IConfiguration> _configuration;
    private readonly LoginUserHandler _handler;
    
    public LoginUserHandlerTests()
    {
        _userRepository = new Mock<IUserRepository>();
        _passwordHasher = new Mock<IPasswordHasher>();
        _tokenGenerator = new Mock<ITokenGenerator>();
        _configuration = new Mock<IConfiguration>();
        _handler = new LoginUserHandler(_userRepository.Object, _passwordHasher.Object, _tokenGenerator.Object,
            _configuration.Object);
    }

    [Fact]
    public async Task Handle_ShouldLoginUser_WhenCredentialsAreValid()
    {
        var command = new LoginUserCommand("test@test.com", "password");
        _userRepository
            .Setup(repo => repo.GetByEmailAsync(command.Email))
            .ReturnsAsync(new User(command.Email, "123", ""));
        _passwordHasher
            .Setup(hasher => hasher.VerifyHash("123", command.Password))
            .ReturnsAsync(true);
        _tokenGenerator
            .Setup(gen => gen.GenerateToken(It.IsAny<string>(), It.IsAny<Guid>()))
            .ReturnsAsync("!@#");
        _configuration
            .Setup(configuration => configuration["JWT:Key"])
            .Returns("JWT");
        
        var result = await _handler.Handle(command, new CancellationToken());
        
        result.IsSuccess.Should().BeTrue();
        _userRepository.Verify(repo => repo.GetByEmailAsync(It.IsAny<string>()), Times.Once);
        _passwordHasher.Verify(hasher => hasher.VerifyHash(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        _tokenGenerator.Verify(gen => gen.GenerateToken(It.IsAny<string>(), It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldNotLoginUser_WhenCredentialsAreInvalid()
    {
        var command = new LoginUserCommand("test@test.com", "password");
        _userRepository
            .Setup(repo => repo.GetByEmailAsync(command.Email))
            .ReturnsAsync(new User(command.Email, "123", ""));
        _passwordHasher
            .Setup(hasher => hasher.VerifyHash("123", command.Password))
            .ReturnsAsync(false);
        _tokenGenerator
            .Setup(gen => gen.GenerateToken(It.IsAny<string>(), It.IsAny<Guid>()))
            .ReturnsAsync("!@#");
        _configuration
            .Setup(configuration => configuration["JWT:Key"])
            .Returns("JWT");
        
        var result = await _handler.Handle(command, new CancellationToken());
        
        result.IsSuccess.Should().BeFalse();
        _userRepository.Verify(repo => repo.GetByEmailAsync(It.IsAny<string>()), Times.Once);
        _passwordHasher.Verify(hasher => hasher.VerifyHash(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        _tokenGenerator.Verify(gen => gen.GenerateToken(It.IsAny<string>(), It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldNotLoginUser_WhenUserDoesNotExist()
    {
        var command = new LoginUserCommand("test@test.com", "password");
        _userRepository
            .Setup(repo => repo.GetByEmailAsync(command.Email))
            .ReturnsAsync(User.Empty);
        _passwordHasher
            .Setup(hasher => hasher.HashPassword(command.Password))
            .ReturnsAsync("123");
        _tokenGenerator
            .Setup(gen => gen.GenerateToken(It.IsAny<string>(), It.IsAny<Guid>()))
            .ReturnsAsync("!@#");
        _configuration
            .Setup(configuration => configuration["JWT:Key"])
            .Returns("JWT");
        
        var result = await _handler.Handle(command, new CancellationToken());
        
        result.IsSuccess.Should().BeFalse();
        _userRepository.Verify(repo => repo.GetByEmailAsync(It.IsAny<string>()), Times.Once);
        _passwordHasher.Verify(hasher => hasher.HashPassword(It.IsAny<string>()), Times.Never);
        _tokenGenerator.Verify(gen => gen.GenerateToken(It.IsAny<string>(), It.IsAny<Guid>()), Times.Never);
    }
}