namespace UserService.Application.Interfaces;

public interface ITokenGenerator
{
    Task<string> GenerateToken(string key, Guid userId, string role);
    Task<bool> ValidateToken(string token, string key);
}