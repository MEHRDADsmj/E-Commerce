using FluentAssertions;
using Moq;
using OrderService.Application.Orders.Commands.GetOrderById;
using OrderService.Domain.Entities;
using OrderService.Domain.Interfaces;

namespace Tests.OrderService;

public class GetOrderByIdHandlerTests
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly GetOrderByIdHandler _handler;

    public GetOrderByIdHandlerTests()
    {
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _handler = new GetOrderByIdHandler(_orderRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnFalse_WhenOrderIsNotFound()
    {
        _orderRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                            .ReturnsAsync(null as Order);

        var query = new GetOrderByIdQuery(Guid.NewGuid());
        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        _orderRepositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnTrue_WhenOrderIsFound()
    {
        _orderRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                            .ReturnsAsync(new Order(Guid.NewGuid(), new List<OrderItem>()));

        var query = new GetOrderByIdQuery(Guid.NewGuid());
        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        _orderRepositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }
}