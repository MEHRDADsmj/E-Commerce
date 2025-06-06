using MediatR;
using OrderService.Domain.Interfaces;
using Shared.Data;

namespace OrderService.Application.Orders.Commands.MarkOrderAsFailed;

public class MarkOrderAsFailedHandler : IRequestHandler<MarkOrderAsFailedCommand, Result<bool>>
{
    private readonly IOrderRepository _orderRepository;

    public MarkOrderAsFailedHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    
    public async Task<Result<bool>> Handle(MarkOrderAsFailedCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.Id);
        if (order == null)
        {
            return Result<bool>.Failure("Order not found");
        }
        
        order.MarkAsFailed();
        await _orderRepository.SaveAsync();
        
        return Result<bool>.Success(true);
    }
}