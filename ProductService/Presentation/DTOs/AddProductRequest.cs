namespace ProductService.Presentation.DTOs;

public record AddProductRequestDto(string Name, decimal UnitPrice, string? Description);