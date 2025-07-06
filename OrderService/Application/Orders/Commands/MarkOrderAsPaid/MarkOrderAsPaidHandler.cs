using MediatR;
using OrderService.Domain.Interfaces;
using Shared.Data;

namespace OrderService.Application.Orders.Commands.MarkOrderAsPaid;

public class MarkOrderAsPaidHandler : IRequestHandler<MarkOrderAsPaidCommand, Result<bool>>
{
    private readonly IOrderRepository _orderRepository;

    public MarkOrderAsPaidHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    
    public async Task<Result<bool>> Handle(MarkOrderAsPaidCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId);
        if (order == null)
        {
            return Result<bool>.Failure("Order not found");
        }
        
        order.MarkAsPaid();
        await _orderRepository.SaveAsync();
        
        return Result<bool>.Success(true);
    }
}