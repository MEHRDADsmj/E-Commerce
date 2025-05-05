namespace UserService.Application.Users.Commands.LoginUser;

public record LoginUserResult(Guid Id, string Email, string Token);