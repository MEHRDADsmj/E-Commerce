using System.ComponentModel.DataAnnotations;

namespace UserService.Presentation.DTOs;

public class LoginUserRequestDto
{
    [Required, EmailAddress, StringLength(64)] public string Email { get; }
    [Required, StringLength(64)] public string Password { get; }

    public LoginUserRequestDto(string email, string password)
    {
        var parts = email.Split("@");
        Email = parts[0].Trim().Replace(".", "") + "@" + parts[1];
        Password = password;
    }
}