using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces;

namespace UserService.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserDbContext _context;
    
    public UserRepository(UserDbContext context)
    {
        _context = context;
    }
    
    public async Task<User> GetByIdAsync(Guid id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user => user.Id == id) ?? User.Empty();
        return user;
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user => user.Email.ToUpper() == email.ToUpper()) ?? User.Empty();
        return user;
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }
}