using MediatR;
using Shared.Data;
using UserService.Application.Interfaces;
using UserService.Domain.Interfaces;

namespace UserService.Application.Users.Commands.LoginUser;

public class LoginUserHandler : IRequestHandler<LoginUserCommand, Result<LoginUserResult>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IConfiguration _configuration;
    
    public LoginUserHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, ITokenGenerator tokenGenerator,
        IConfiguration configuration)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenGenerator = tokenGenerator;
        _configuration = configuration;
    }

    public async Task<Result<LoginUserResult>> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(command.Email);
        if (user.IsEmpty())
        {
            return Result<LoginUserResult>.Failure("Invalid username or password");
        }
        
        if (!(await _passwordHasher.VerifyHash(user.HashedPassword, command.Password)))
        {
            return Result<LoginUserResult>.Failure("Invalid username or password");
        }

        var result = new LoginUserResult(user.Id, user.Email,
                                         await _tokenGenerator.GenerateToken(_configuration["Jwt:Key"], user.Id, user.Role));
        return Result<LoginUserResult>.Success(result);
    }
}