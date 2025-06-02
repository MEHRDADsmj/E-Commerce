using MediatR;
using ProductService.Domain.Entities;
using ProductService.Domain.Interfaces;
using Shared.Data;

namespace ProductService.Application.Products.Commands.AddProduct;

public class AddProductHandler : IRequestHandler<AddProductCommand, Result<Product>>
{
    private readonly IProductRepository _productRepository;

    public AddProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    
    public async Task<Result<Product>> Handle(AddProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product(request.Name, request.UnitPrice, request.Description);
        try
        {
            await _productRepository.AddAsync(product);
            return Result<Product>.Success(product);
        }
        catch (Exception ex)
        {
            return Result<Product>.Failure(ex.Message);
        }
    }
}