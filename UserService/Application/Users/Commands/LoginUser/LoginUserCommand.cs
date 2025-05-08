using MediatR;
using Shared.Data;

namespace UserService.Application.Users.Commands.LoginUser;

public record LoginUserCommand(string Email, string Password) : IRequest<Result<LoginUserResult>>;