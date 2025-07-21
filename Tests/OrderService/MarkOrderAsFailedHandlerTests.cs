using FluentAssertions;
using Moq;
using OrderService.Application.Orders.Commands.MarkOrderAsFailed;
using OrderService.Domain.Entities;
using OrderService.Domain.Interfaces;

namespace Tests.OrderService;

public class MarkOrderAsFailedHandlerTests
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly MarkOrderAsFailedHandler _handler;

    public MarkOrderAsFailedHandlerTests()
    {
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _handler = new MarkOrderAsFailedHandler(_orderRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnFalse_WhenOrderIsNotFound()
    {
        _orderRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                            .ReturnsAsync(Order.Empty);
        
        var command = new MarkOrderAsFailedCommand(Guid.NewGuid());
        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        _orderRepositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnTrue_WhenOrderIsFound()
    {
        var order = new Order(Guid.NewGuid(), new List<OrderItem>());
        _orderRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                            .ReturnsAsync(order);
        
        var command = new MarkOrderAsFailedCommand(Guid.NewGuid());
        var result = await _handler.Handle(command, CancellationToken.None);
        
        result.IsSuccess.Should().BeTrue();
        order.Status.Should().Be(OrderStatus.Failed);
        _orderRepositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }
}