using System.ComponentModel.DataAnnotations;

namespace UserService.Presentation.DTOs;

public class RegisterUserRequestDto
{
    [Required, EmailAddress, StringLength(64)] public string Email { get; }
    [Required, StringLength(64)] public string Password { get; }
    [Required, StringLength(64)] public string FullName { get; }

    public RegisterUserRequestDto(string email, string password, string fullName)
    {
        var parts = email.Split("@");
        Email = parts[0].Trim().Replace(".", "") + "@" + parts[1];
        Password = password;
        FullName = fullName;
    }
}