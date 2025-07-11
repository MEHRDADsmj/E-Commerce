using System.ComponentModel.DataAnnotations;

namespace UserService.Presentation.DTOs;

public class RegisterUserRequestDto
{
    [EmailAddress] public required string Email { get; init; }
    public required string Password { get; init; }
    [Length(1, 64)] public required string FullName { get; init; }

    public RegisterUserRequestDto(string email, string password, string fullName)
    {
        Email = email;
        Password = password;
        FullName = fullName;
        ValidationContext validationContext = new ValidationContext(this);
        Validator.ValidateObject(this, validationContext, true);
    }
}