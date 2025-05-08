namespace UserService.Application.Interfaces;

public interface IPasswordHasher
{
    Task<string> HashPassword(string password);
    Task<bool> VerifyHash(string hashedPassword, string providedPassword);
}