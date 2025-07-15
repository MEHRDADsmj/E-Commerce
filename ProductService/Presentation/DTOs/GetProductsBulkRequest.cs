namespace ProductService.Presentation.DTOs;

public record GetProductsBulkRequestDto(IEnumerable<Guid> ProductIds);