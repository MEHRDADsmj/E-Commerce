using MediatR;
using OrderService.Contracts.DTOs;
using OrderService.Domain.Entities;
using OrderService.Domain.Interfaces;
using Shared.Data;

namespace OrderService.Application.Orders.Commands.GetOrderByUserId;

public class GetOrderByUserIdHandler : IRequestHandler<GetOrderByUserIdQuery, Result<IEnumerable<OrderDto>>>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderByUserIdHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    
    public async Task<Result<IEnumerable<OrderDto>>> Handle(GetOrderByUserIdQuery request, CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.GetByUserIdAsync(request.UserId);
        if (orders.Count == 0)
        {
            return Result<IEnumerable<OrderDto>>.Failure("Orders not found");
        }

        var orderDtos = new List<OrderDto>();
        foreach (var order in orders)
        {
            var orderItemDtos = order.Items.Select(item => new OrderItemDto()
                                                           {
                                                               ProductId = item.ProductId, 
                                                               Quantity = item.Quantity,
                                                               UnitPrice = item.UnitPrice,
                                                               ProductName = item.ProductName,
                                                           });
            var orderDto = new OrderDto()
                           {
                               Id = order.Id,
                               UserId = order.UserId,
                               Status = order.Status,
                               CreatedAt = order.CreatedAt,
                               TotalPrice = order.TotalPrice,
                               Items = orderItemDtos
                           };
            orderDtos.Add(orderDto);
        }
        
        return Result<IEnumerable<OrderDto>>.Success(orderDtos);
    }
}