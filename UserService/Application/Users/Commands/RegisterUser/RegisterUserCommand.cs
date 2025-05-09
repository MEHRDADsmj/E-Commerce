using MediatR;
using Shared.Data;

namespace UserService.Application.Users.Commands.RegisterUser;

public record RegisterUserCommand(string Email, string Password, string FullName) : IRequest<Result<Guid>>;