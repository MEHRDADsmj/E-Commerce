using MediatR;
using OrderService.Contracts.DTOs;
using OrderService.Domain.Entities;
using OrderService.Domain.Interfaces;
using Shared.Data;

namespace OrderService.Application.Orders.Commands.GetOrderById;

public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, Result<OrderDto>>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderByIdHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    
    public async Task<Result<OrderDto>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId);
        if (order.IsEmpty())
        {
            return Result<OrderDto>.Failure("Order not found");
        }

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
        return Result<OrderDto>.Success(orderDto);
    }
}