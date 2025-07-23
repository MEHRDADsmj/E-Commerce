using CartService.Domain.Entities;
using CartService.Domain.Interfaces;
using MediatR;
using Shared.Data;

namespace CartService.Application.Carts.Commands.GetCart;

public class GetCartHandler : IRequestHandler<GetCartQuery, Result<Cart>>
{
    private readonly ICartRepository _cartRepository;

    public GetCartHandler(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task<Result<Cart>> Handle(GetCartQuery query, CancellationToken token)
    {
        var cart = await _cartRepository.GetAsync(query.UserId);
        if (cart.IsEmpty())
        {
            return Result<Cart>.Failure("Cart not found");
        }
        return Result<Cart>.Success(cart);
    }
}