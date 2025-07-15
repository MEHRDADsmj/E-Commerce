using MediatR;
using Shared.Data;

namespace UserService.Application.Users.Queries.GetUserProfile;

public record GetUserProfileQuery(Guid UserId) : IRequest<Result<GetUserProfileResult>>;