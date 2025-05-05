using Shared.Data;
using UserService.Application.Interfaces;
using UserService.Domain.Interfaces;

namespace UserService.Application.Users.Commands.LoginUser;

public class LoginUserHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenGenerator _tokenGenerator;
    
    public LoginUserHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, ITokenGenerator tokenGenerator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<Result<LoginUserResult>> Handle(LoginUserCommand command)
    {
        var user = await _userRepository.GetByEmailAsync(command.Email);
        if (user == null)
        {
            return Result<LoginUserResult>.Failure("Invalid username or password");
        }
        var passwordHash = await _passwordHasher.HashPassword(command.Password);
        if (!string.Equals(passwordHash, user.HashedPassword))
        {
            return Result<LoginUserResult>.Failure("Invalid username or password");
        }

        var result = new LoginUserResult(user.Id, user.Email, "ABC123"); // TODO: Replace with real token
        return Result<LoginUserResult>.Success(result);
    }
}