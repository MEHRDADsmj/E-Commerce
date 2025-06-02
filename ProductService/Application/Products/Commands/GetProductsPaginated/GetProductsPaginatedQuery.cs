using MediatR;
using ProductService.Domain.Entities;
using Shared.Data;

namespace ProductService.Application.Products.Commands.GetProductsPaginated;

public record GetProductsPaginatedQuery(int Page, int PageSize) : IRequest<Result<IEnumerable<Product>>>;