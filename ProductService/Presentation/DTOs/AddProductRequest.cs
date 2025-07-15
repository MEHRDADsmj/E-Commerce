using System.ComponentModel.DataAnnotations;

namespace ProductService.Presentation.DTOs;

public record AddProductRequestDto
{
    [Required, StringLength(64)] public string Name { get; }
    [Required, Range(0, double.MaxValue)] public decimal UnitPrice { get; }
    [StringLength(256)] public string? Description { get; }

    public AddProductRequestDto(string name, string description, decimal unitPrice)
    {
        Name = name;
        Description = description;
        UnitPrice = unitPrice;
    }
}