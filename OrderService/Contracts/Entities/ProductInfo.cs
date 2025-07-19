using System.ComponentModel.DataAnnotations;

namespace OrderService.Contracts.Entities;

public class ProductInfo
{
    [Required] public Guid Id { get; init; }
    [Required, StringLength(256)] public string Name { get; init; }
    [Required, Range(1, double.MaxValue)] public decimal UnitPrice { get; init; }

    public ProductInfo(Guid id, string name, decimal unitPrice)
    {
        Id = id;
        Name = name;
        UnitPrice = unitPrice;
        ValidationContext validationContext = new ValidationContext(this);
        Validator.ValidateObject(this, validationContext, true);
    }
}