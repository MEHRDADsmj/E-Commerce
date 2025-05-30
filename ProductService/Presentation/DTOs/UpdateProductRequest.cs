namespace ProductService.Presentation.DTOs;

public record UpdateProductRequestDto(Guid Id, string Name, decimal UnitPrice, string? Description);