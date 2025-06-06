using MediatR;
using OrderService.Domain.Entities;
using OrderService.Domain.Interfaces;
using Shared.Data;

namespace OrderService.Application.Orders.Commands.GetOrderByUserId;

public class GetOrderByUserIdHandler : IRequestHandler<GetOrderByUserIdQuery, Result<IEnumerable<Order>>>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderByUserIdHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    
    public async Task<Result<IEnumerable<Order>>> Handle(GetOrderByUserIdQuery request, CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.GetByUserIdAsync(request.UserId);
        if (orders.Count == 0)
        {
            return Result<IEnumerable<Order>>.Failure("Orders not found");
        }
        
        return Result<IEnumerable<Order>>.Success(orders);
    }
}