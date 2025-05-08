namespace UserService.Application.Interfaces;

public interface ITokenGenerator
{
    Task<string> GenerateToken(string key);
    Task<bool> ValidateToken(string token, string key);
}