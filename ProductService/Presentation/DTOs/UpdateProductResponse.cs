namespace ProductService.Presentation.DTOs;

public record UpdateProductResponseDto(Guid Id, string Name, decimal UnitPrice, string? Description);