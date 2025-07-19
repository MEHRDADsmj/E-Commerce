using MediatR;
using OrderService.Application.Interfaces;
using OrderService.Contracts.Entities;
using OrderService.Contracts.Events;
using OrderService.Domain.Entities;
using OrderService.Domain.Interfaces;
using Shared.Data;

namespace OrderService.Application.Orders.Commands.CreateOrder;

public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Result<Guid>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICartClient _cartClient;
    private readonly IEventPublisher _eventPublisher;
    private readonly IProductClient _productClient;

    public CreateOrderHandler(IOrderRepository orderRepository, ICartClient cartClient, IEventPublisher eventPublisher,
                              IProductClient productClient)
    {
        _orderRepository = orderRepository;
        _cartClient = cartClient;
        _eventPublisher = eventPublisher;
        _productClient = productClient;
    }
    
    public async Task<Result<Guid>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartClient.GetCartAsync(request.Token);
        if (cart.IsEmpty() || cart.Items.Count == 0)
        {
            return Result<Guid>.Failure("Cart is empty");
        }

        var products = await GetProductsAsDictionary(cart.Items, request.Token);

        List<OrderItem> orderItems;
        try
        {
            orderItems = CreateOrderItems(cart.Items, products).ToList();
        }
        catch (Exception ex)
        {
            return Result<Guid>.Failure(ex.Message);
        }

        var order = new Order(cart.UserId, orderItems);
        await _orderRepository.AddAsync(order);
        await _orderRepository.SaveAsync();

        await _eventPublisher.PublishAsync(new OrderCreatedEvent(order.Id, order.UserId, order.TotalPrice));
        
        return Result<Guid>.Success(order.Id);
    }

    private async Task<Dictionary<Guid, ProductInfo>> GetProductsAsDictionary(List<CartItem> items, string token)
    {
        var productIds = items.Select((item => item.ProductId)).Distinct();
        var products = await GetProductsBulk(productIds.ToList(), token);
        return products.ToDictionary(p => p.Id);
    }
    
    private async Task<IEnumerable<ProductInfo>> GetProductsBulk(List<Guid> productIds, string token)
    {
        return await _productClient.GetProducts(productIds, token);
    }

    private IEnumerable<OrderItem> CreateOrderItems(List<CartItem> items, Dictionary<Guid, ProductInfo> productInfos)
    {
        return items.Select(item =>
                          {
                              if (!productInfos.TryGetValue(item.ProductId, out var product))
                              {
                                  throw new Exception($"Product {item.ProductId} not found");
                              }

                              return new OrderItem(product.Id, product.Name, product.UnitPrice,
                                                   item.Quantity);
                          });
    }
}