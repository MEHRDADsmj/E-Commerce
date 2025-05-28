using MediatR;
using ProductService.Domain.Entities;
using Shared.Data;

namespace ProductService.Application.Products.Commands.UpdateProduct;

public record UpdateProductCommand(Product Product) : IRequest<Result<Product>>;