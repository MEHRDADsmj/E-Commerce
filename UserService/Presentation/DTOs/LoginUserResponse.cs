namespace UserService.Presentation.DTOs;

public record LoginUserResponseDto(Guid Id, string Email, string AccessToken);