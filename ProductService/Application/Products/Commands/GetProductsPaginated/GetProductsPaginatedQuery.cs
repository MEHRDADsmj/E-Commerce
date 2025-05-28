using MediatR;
using ProductService.Domain.Entities;
using Shared.Data;

namespace ProductService.Application.Products.Commands.GetProductsPaginated;

public record GetProductsPaginatedQuery(uint Page, uint PageSize) : IRequest<Result<IEnumerable<Product>>>;