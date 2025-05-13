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

    public async Task InitializeAsync()
    {
        _dbContainer = new PostgreSqlBuilder().Build();
        await _dbContainer.StartAsync();

        var options = new DbContextOptionsBuilder<UserDbContext>();
        options.UseNpgsql(_dbContainer.GetConnectionString());

        var dbContext = new UserDbContext(options.Options);
        await dbContext.Database.MigrateAsync();
        _userRepository = new UserRepository(dbContext);
    }

    [Fact]
    public async Task AddAsync_ShouldAddUser()
    {
        var user = new User()
                   {
                       Email = "test@test.com",
                       Id = Guid.NewGuid(),
                       CreatedAt = DateTime.UtcNow,
                       FullName = "John Doe",
                       HashedPassword = "password",
                   };
        await _userRepository.AddAsync(user);
        var result = await _userRepository.GetByIdAsync(user.Id);
        Assert.NotNull(result);
        Assert.Equal(user.Email, result.Email);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnUser()
    {
        var user = new User()
                   {
                       Email = "test@test.com",
                       Id = Guid.NewGuid(),
                       CreatedAt = DateTime.UtcNow,
                       FullName = "John Doe",
                       HashedPassword = "password",
                   };
        await _userRepository.AddAsync(user);
        var result = await _userRepository.GetByIdAsync(user.Id);
        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
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
        var user = new User()
                   {
                       Email = "test@test.com",
                       Id = Guid.NewGuid(),
                       CreatedAt = DateTime.UtcNow,
                       FullName = "John Doe",
                       HashedPassword = "password",
                   };
        await _userRepository.AddAsync(user);
        var result = await _userRepository.GetByEmailAsync(user.Email);
        Assert.NotNull(result);
        Assert.Equal(user.Email, result.Email);
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnNull()
    {
        var result = await _userRepository.GetByEmailAsync("test@test.com");
        Assert.Null(result);
    }

    public Task DisposeAsync()
    {
        return _dbContainer.DisposeAsync().AsTask();
    }
}