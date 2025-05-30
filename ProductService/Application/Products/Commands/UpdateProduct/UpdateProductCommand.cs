using MediatR;
using ProductService.Domain.Entities;
using Shared.Data;

namespace ProductService.Application.Products.Commands.UpdateProduct;

public record UpdateProductCommand(ProductDto Product) : IRequest<Result<Product>>;