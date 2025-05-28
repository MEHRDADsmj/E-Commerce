using MediatR;
using ProductService.Domain.Entities;
using ProductService.Domain.Interfaces;
using Shared.Data;

namespace ProductService.Application.Products.Commands.GetProductsPaginated;

public class GetProductsPaginatedHandler : IRequestHandler<GetProductsPaginatedQuery, Result<IEnumerable<Product>>>
{
    private readonly IProductRepository _productRepository;

    public GetProductsPaginatedHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    
    public async Task<Result<IEnumerable<Product>>> Handle(GetProductsPaginatedQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetAllAsync(request.Page, request.PageSize);
        if (products.Count == 0)
        {
            return Result<IEnumerable<Product>>.Failure("No products found on this page");
        }
        return Result<IEnumerable<Product>>.Success(products);
    }
}