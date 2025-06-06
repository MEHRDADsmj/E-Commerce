namespace OrderService.Contracts.Entities;

public class ProductInfo
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal UnitPrice { get; set; }
}