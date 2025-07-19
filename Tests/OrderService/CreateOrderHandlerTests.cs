using Bogus;
using FluentAssertions;
using Moq;
using OrderService.Application.Interfaces;
using OrderService.Application.Orders.Commands.CreateOrder;
using OrderService.Contracts.Entities;
using OrderService.Contracts.Events;
using OrderService.Domain.Entities;
using OrderService.Domain.Interfaces;

namespace Tests.OrderService;

public class CreateOrderHandlerTests
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly Mock<ICartClient> _cartClientMock;
    private readonly Mock<IEventPublisher> _eventPublisherMock;
    private readonly Mock<IProductClient> _productClientMock;
    private readonly CreateOrderHandler _handler;

    public CreateOrderHandlerTests()
    {
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _cartClientMock = new Mock<ICartClient>();
        _eventPublisherMock = new Mock<IEventPublisher>();
        _productClientMock = new Mock<IProductClient>();
        _handler = new CreateOrderHandler(_orderRepositoryMock.Object, _cartClientMock.Object,
                                          _eventPublisherMock.Object, _productClientMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnFalse_WhenCartIsNull()
    {
        _cartClientMock.Setup(client => client.GetCartAsync(It.IsAny<string>()))
                       .ReturnsAsync(Cart.Empty);

        var command = new CreateOrderCommand(Guid.NewGuid(), new Faker().Random.String());
        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        _cartClientMock.Verify(client => client.GetCartAsync(It.IsAny<string>()), Times.Once);
        _orderRepositoryMock.VerifyNoOtherCalls();
        _eventPublisherMock.VerifyNoOtherCalls();
        _productClientMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Handle_ShouldReturnFalse_WhenCartIsEmpty()
    {
        _cartClientMock.Setup(client => client.GetCartAsync(It.IsAny<string>()))
                       .ReturnsAsync(new Cart(Guid.NewGuid(), new List<CartItem>()));

        var command = new CreateOrderCommand(Guid.NewGuid(), new Faker().Random.String());
        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        _cartClientMock.Verify(client => client.GetCartAsync(It.IsAny<string>()), Times.Once);
        _orderRepositoryMock.VerifyNoOtherCalls();
        _eventPublisherMock.VerifyNoOtherCalls();
        _productClientMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Handle_ShouldReturnFalse_WhenProductsNotExist()
    {
        _cartClientMock.Setup(client => client.GetCartAsync(It.IsAny<string>()))
                       .ReturnsAsync(new Cart(Guid.NewGuid(), new Faker<CartItem>().RuleFor(item => item.ProductId, Guid.NewGuid())
                                                                                   .RuleFor(item => item.Quantity, new Faker().Random.Int(1, 10))
                                                                                   .Generate(3)));
        _productClientMock.Setup(client => client.GetProducts(It.IsAny<List<Guid>>(), It.IsAny<string>()))
                          .ReturnsAsync(new List<ProductInfo>());
        
        var command = new CreateOrderCommand(Guid.NewGuid(), new Faker().Random.String());
        var result = await _handler.Handle(command, CancellationToken.None);
        
        result.IsSuccess.Should().BeFalse();
        _cartClientMock.Verify(client => client.GetCartAsync(It.IsAny<string>()), Times.Once);
        _productClientMock.Verify(client => client.GetProducts(It.IsAny<List<Guid>>(), It.IsAny<string>()), Times.Once);
        _orderRepositoryMock.VerifyNoOtherCalls();
        _eventPublisherMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Handle_ShouldReturnFalse_WhenProductsDoNotMatch()
    {
        _cartClientMock.Setup(client => client.GetCartAsync(It.IsAny<string>()))
                       .ReturnsAsync(new Cart(Guid.NewGuid(), new Faker<CartItem>().RuleFor(item => item.ProductId, Guid.NewGuid())
                                                                                   .RuleFor(item => item.Quantity, new Faker().Random.Int(1, 10))
                                                                                   .Generate(3)));
        _productClientMock.Setup(client => client.GetProducts(It.IsAny<List<Guid>>(), It.IsAny<string>()))
                          .ReturnsAsync(new List<ProductInfo>() { new ProductInfo() { Id = Guid.NewGuid() } });
        
        var command = new CreateOrderCommand(Guid.NewGuid(), new Faker().Random.String());
        var result = await _handler.Handle(command, CancellationToken.None);
        
        result.IsSuccess.Should().BeFalse();
        _cartClientMock.Verify(client => client.GetCartAsync(It.IsAny<string>()), Times.Once);
        _productClientMock.Verify(client => client.GetProducts(It.IsAny<List<Guid>>(), It.IsAny<string>()), Times.Once);
        _orderRepositoryMock.VerifyNoOtherCalls();
        _eventPublisherMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Handle_ShouldReturnTrue_OrderIsCreated()
    {
        var guid = Guid.NewGuid();
        _cartClientMock.Setup(client => client.GetCartAsync(It.IsAny<string>()))
                       .ReturnsAsync(new Cart(Guid.NewGuid(), new List<CartItem>() { new CartItem(guid, 1) }));
        _productClientMock.Setup(client => client.GetProducts(It.IsAny<List<Guid>>(), It.IsAny<string>()))
                          .ReturnsAsync(new List<ProductInfo>() { new ProductInfo() { Id = guid, UnitPrice = 200 } });
        _orderRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Order>()))
                            .Returns(Task.CompletedTask);
        _orderRepositoryMock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);
        _eventPublisherMock.Setup(publisher => publisher.PublishAsync(It.IsAny<OrderCreatedEvent>()))
                           .Returns(Task.CompletedTask);
        
        var command = new CreateOrderCommand(Guid.NewGuid(), new Faker().Random.String());
        var result = await _handler.Handle(command, CancellationToken.None);
        
        result.IsSuccess.Should().BeTrue();
        _orderRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Order>()), Times.Once);
        _orderRepositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
        _eventPublisherMock.Verify(publisher => publisher.PublishAsync(It.IsAny<OrderCreatedEvent>()), Times.Once);
        _cartClientMock.Verify(client => client.GetCartAsync(It.IsAny<string>()), Times.Once);
        _productClientMock.Verify(client => client.GetProducts(It.IsAny<List<Guid>>(), It.IsAny<string>()), Times.Once);
    }
}