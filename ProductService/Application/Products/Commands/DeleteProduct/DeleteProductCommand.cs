using MediatR;
using Shared.Data;

namespace ProductService.Application.Products.Commands.DeleteProduct;

public record DeleteProductCommand(Guid ProductId) : IRequest<Result<bool>>;