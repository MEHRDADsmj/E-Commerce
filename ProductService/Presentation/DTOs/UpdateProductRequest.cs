using System.ComponentModel.DataAnnotations;

namespace ProductService.Presentation.DTOs;

public class UpdateProductRequestDto
{
    [Required] public Guid Id { get; }
    [StringLength(64)] public string? Name { get; }
    [Range(0, double.MaxValue)] public decimal? UnitPrice { get; }
    [StringLength(256)] public string? Description { get; }

    public UpdateProductRequestDto(Guid id, string? name, decimal? unitPrice, string? description)
    {
        Id = id;
        Name = name;
        UnitPrice = unitPrice;
        Description = description;
    }
}