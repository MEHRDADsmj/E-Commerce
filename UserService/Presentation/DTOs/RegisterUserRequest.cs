namespace UserService.Presentation.DTOs;

public record RegisterUserRequestDto(string Email, string Password, string FullName);