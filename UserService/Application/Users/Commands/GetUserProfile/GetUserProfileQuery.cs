using MediatR;
using Shared.Data;

namespace UserService.Application.Users.Commands.GetUserProfile;

public record GetUserProfileQuery(Guid UserId) : IRequest<Result<GetUserProfileResult>>;