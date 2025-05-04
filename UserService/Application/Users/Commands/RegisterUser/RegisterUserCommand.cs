namespace UserService.Application.Users.Commands.RegisterUser;

public record RegisterUserCommand(string Email, string Password, string FullName);