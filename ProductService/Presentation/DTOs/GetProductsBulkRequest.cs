namespace ProductService.Presentation.DTOs;

public record GetProductsBulkRequestDto(List<Guid> ProductIds);