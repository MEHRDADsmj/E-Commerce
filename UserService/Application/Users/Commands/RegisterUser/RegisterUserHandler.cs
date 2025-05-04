using System.Data;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces;
using Shared.Data;

namespace UserService.Application.Users.Commands.RegisterUser;

public class RegisterUserHandler
{
    private readonly IUserRepository _userRepository;
    
    public RegisterUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
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
                          HashedPassword = command.Password, // TODO: Hash the password
                          CreatedAt = DateTime.UtcNow,
                      };
        await _userRepository.AddAsync(newUser);
        return Result<Guid>.Success(newUser.Id);
    }
}