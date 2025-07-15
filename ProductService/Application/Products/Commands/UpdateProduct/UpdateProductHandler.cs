using MediatR;
using ProductService.Domain.Entities;
using ProductService.Domain.Interfaces;
using Shared.Data;

namespace ProductService.Application.Products.Commands.UpdateProduct;

public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, Result<Product>>
{
    private readonly IProductRepository _productRepository;

    public UpdateProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    
    public async Task<Result<Product>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var oldProduct = await _productRepository.GetByIdAsync(request.Product.Id);
        if (oldProduct.IsEmpty())
        {
            return Result<Product>.Failure("Product not found");
        }

        try
        {
            var res = await _productRepository.UpdateAsync(request.Product);
            return Result<Product>.Success(res);
        }
        catch (Exception ex)
        {
            return Result<Product>.Failure(ex.Message);
        }
    }
}