using CartService.Domain.Interfaces;
using MediatR;
using Shared.Data;

namespace CartService.Application.Carts.Commands.RemoveItemFromCart;

public class RemoveItemFromCartHandler : IRequestHandler<RemoveItemFromCartCommand, Result<bool>>
{
    private readonly ICartRepository _cartRepository;

    public RemoveItemFromCartHandler(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task<Result<bool>> Handle(RemoveItemFromCartCommand command, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetAsync(command.UserId);
        if (cart.IsEmpty())
        {
            return Result<bool>.Failure("Invalid cart");
        }
        
        await _cartRepository.RemoveItemAsync(cart.UserId, command.ProductId);
        return Result<bool>.Success(true);
    }
}