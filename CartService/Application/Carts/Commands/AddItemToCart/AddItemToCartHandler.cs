using CartService.Domain.Interfaces;
using MediatR;
using Shared.Data;

namespace CartService.Application.Carts.Commands.AddItemToCart;

public class AddItemToCartHandler : IRequestHandler<AddItemToCartCommand, Result<bool>>
{
    private readonly ICartRepository _cartRepository;

    public AddItemToCartHandler(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task<Result<bool>> Handle(AddItemToCartCommand command, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetAsync(command.UserId);
        if (cart.IsEmpty())
        {
            cart = await _cartRepository.CreateCartAsync(command.UserId);
            if (cart.IsEmpty())
            {
                return Result<bool>.Failure("Failed to create cart");
            }
        }

        try
        {
            await _cartRepository.AddItemAsync(cart.UserId, command.ProductId, command.Quantity);
            return Result<bool>.Success(true);
        }
        catch (Exception e)
        {
            return Result<bool>.Failure(e.Message);
        }
    }
}