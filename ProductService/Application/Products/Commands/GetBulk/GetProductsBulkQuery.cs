using MediatR;
using ProductService.Domain.Entities;
using Shared.Data;

namespace ProductService.Application.Products.Commands.GetBulk;

public record GetProductsBulkQuery(IEnumerable<Guid> Products) : IRequest<Result<IEnumerable<Product>>>;