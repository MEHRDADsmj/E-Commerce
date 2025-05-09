using MediatR;
using Shared.Data;

namespace UserService.Application.Users.Commands.GetUserProfile;

public record GetUserProfileCommand(Guid UserId) : IRequest<Result<GetUserProfileResult>>;