using UserService.Application.Interfaces;

namespace UserService.Infrastructure;

public class BCryptPasswordHasher : IPasswordHasher
{
    public async Task<string> HashPassword(string password)
    {
        await Task.CompletedTask;
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public async Task<bool> VerifyHash(string hashedPassword, string providedPassword)
    {
        await Task.CompletedTask;
        return BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);
    }
}