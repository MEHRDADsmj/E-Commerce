using MediatR;
using ProductService.Domain.Entities;
using Shared.Data;

namespace ProductService.Application.Products.Commands.GetProductById;

public record GetProductByIdQuery(Guid Id) : IRequest<Result<Product>>;