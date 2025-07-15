using MediatR;
using ProductService.Domain.Entities;
using ProductService.Domain.Interfaces;
using Shared.Data;

namespace ProductService.Application.Products.Commands.GetBulk;

public class GetProductsBulkHandler : IRequestHandler<GetProductsBulkQuery, Result<IEnumerable<Product>>>
{
    private readonly IProductRepository _productRepository;

    public GetProductsBulkHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    
    public async Task<Result<IEnumerable<Product>>> Handle(GetProductsBulkQuery request, CancellationToken cancellationToken)
    {
        if (!request.Products.Any())
        {
            return Result<IEnumerable<Product>>.Failure("No Ids provided");
        }
        var products = await _productRepository.GetBulkAsync(request.Products);
        var enumerable = products as Product[] ?? products.ToArray();
        if (enumerable.Length == 0)
        {
            return Result<IEnumerable<Product>>.Failure("No Products found");
        }
        return Result<IEnumerable<Product>>.Success(enumerable);
    }
}