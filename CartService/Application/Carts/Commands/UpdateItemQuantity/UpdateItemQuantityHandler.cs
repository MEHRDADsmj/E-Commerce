using CartService.Domain.Interfaces;
using Shared.Data;

namespace CartService.Application.Carts.Commands.UpdateItemQuantity;

public class UpdateItemQuantityHandler
{
    private readonly ICartRepository _cartRepository;

    public UpdateItemQuantityHandler(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task<Result<bool>> Handle(UpdateItemQuantityCommand command, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetAsync(command.UserId);
        if (cart == null)
        {
            return Result<bool>.Failure("Invalid user");
        }

        try
        {
            await _cartRepository.UpdateItemQuantityAsync(cart.UserId, command.ProductId, command.NewQuantity);
            return Result<bool>.Success(true);
        }
        catch (Exception e)
        {
            return Result<bool>.Failure(e.Message);
        }
    }
}