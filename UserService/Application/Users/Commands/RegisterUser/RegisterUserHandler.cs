using UserService.Domain.Entities;
using UserService.Domain.Interfaces;
using Shared.Data;
using UserService.Application.Interfaces;

namespace UserService.Application.Users.Commands.RegisterUser;

public class RegisterUserHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterUserHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<Guid>> Handle(RegisterUserCommand command)
    {
        var user = await _userRepository.GetByEmailAsync(command.Email);
        if (user != null)
        {
            return Result<Guid>.Failure("User already exists");
        }

        var newUser = new User()
                      {
                          Email = command.Email,
                          FullName = command.FullName,
                          HashedPassword = await _passwordHasher.HashPassword(command.Password),
                          CreatedAt = DateTime.UtcNow,
                      };
        await _userRepository.AddAsync(newUser);
        return Result<Guid>.Success(newUser.Id);
    }
}