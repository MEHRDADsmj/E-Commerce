using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;
using UserService.Data;
using UserService.Domain.Entities;
using UserService.Infrastructure.Repositories;

namespace Tests.UserService;

public class UserRepositoryTests : IAsyncLifetime
{
    private UserRepository _userRepository;
    private PostgreSqlContainer _dbContainer;
    private User _user;

    public async Task InitializeAsync()
    {
        _dbContainer = new PostgreSqlBuilder().WithImage("postgres:14.4-alpine").WithCleanUp(true).Build();
        await _dbContainer.StartAsync();

        var options = new DbContextOptionsBuilder<UserDbContext>();
        options.UseNpgsql(_dbContainer.GetConnectionString());

        var dbContext = new UserDbContext(options.Options);
        await dbContext.Database.MigrateAsync();
        _userRepository = new UserRepository(dbContext);
        _user = new User("test@test.com", "password", "John Doe");
    }

    [Fact]
    public async Task AddAsync_ShouldAddUser()
    {
        await _userRepository.AddAsync(_user);
        var result = await _userRepository.GetByIdAsync(_user.Id);
        Assert.NotNull(result);
        Assert.Equal(_user.Email, result.Email);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnUser()
    {
        await _userRepository.AddAsync(_user);
        var result = await _userRepository.GetByIdAsync(_user.Id);
        Assert.NotNull(result);
        Assert.Equal(_user.Id, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull()
    {
        var result = await _userRepository.GetByIdAsync(Guid.NewGuid());
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnUser()
    {
        await _userRepository.AddAsync(_user);
        var result = await _userRepository.GetByEmailAsync(_user.Email);
        Assert.NotNull(result);
        Assert.Equal(_user.Email, result.Email);
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnNull()
    {
        var result = await _userRepository.GetByEmailAsync("test@test.com");
        Assert.True(result.IsEmpty());
    }

    public Task DisposeAsync()
    {
        return _dbContainer.DisposeAsync().AsTask();
    }
}