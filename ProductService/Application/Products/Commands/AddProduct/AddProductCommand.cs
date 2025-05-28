using MediatR;
using ProductService.Domain.Entities;
using Shared.Data;

namespace ProductService.Application.Products.Commands.AddProduct;

public record AddProductCommand(string Name, decimal UnitPrice, string? Description) : IRequest<Result<Product>>;