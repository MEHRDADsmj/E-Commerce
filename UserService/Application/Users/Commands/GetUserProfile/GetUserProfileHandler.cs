using MediatR;
using Shared.Data;
using UserService.Domain.Interfaces;

namespace UserService.Application.Users.Commands.GetUserProfile;

public class GetUserProfileHandler : IRequestHandler<GetUserProfileQuery, Result<GetUserProfileResult>>
{
    private readonly IUserRepository _userRepository;

    public GetUserProfileHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<Result<GetUserProfileResult>> Handle(GetUserProfileQuery query, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(query.UserId);
        if (user == null)
        {
            return Result<GetUserProfileResult>.Failure("User not found");
        }

        var result = new GetUserProfileResult(user.Email, user.FullName);
        return Result<GetUserProfileResult>.Success(result);
    }
}