namespace UserService.Application.Interfaces;

public interface ITokenGenerator
{
    Task<string> GenerateToken();
}