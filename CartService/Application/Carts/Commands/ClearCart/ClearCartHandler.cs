using CartService.Domain.Interfaces;
using Shared.Data;

namespace CartService.Application.Carts.Commands.ClearCart;

public class ClearCartHandler
{
    private readonly ICartRepository _cartRepository;

    public ClearCartHandler(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task<Result<bool>> Handle(ClearCartCommand command, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetAsync(command.UserId);
        if (cart == null)
        {
            return Result<bool>.Failure("Invalid cart");
        }

        await _cartRepository.ClearCartAsync(cart.UserId);
        return Result<bool>.Success(true);
    }
}