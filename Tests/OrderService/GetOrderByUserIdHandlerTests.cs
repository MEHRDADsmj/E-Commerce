using Bogus;
using FluentAssertions;
using Moq;
using OrderService.Application.Orders.Commands.GetOrderByUserId;
using OrderService.Domain.Entities;
using OrderService.Domain.Interfaces;

namespace Tests.OrderService;

public class GetOrderByUserIdHandlerTests
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly GetOrderByUserIdHandler _handler;

    public GetOrderByUserIdHandlerTests()
    {
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _handler = new GetOrderByUserIdHandler(_orderRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnFalse_WhenOrderIsNotFound()
    {
        _orderRepositoryMock.Setup(repo => repo.GetByUserIdAsync(It.IsAny<Guid>()))
                            .ReturnsAsync(new List<Order>());
        
        var query = new GetOrderByUserIdQuery(Guid.NewGuid());
        var result = await _handler.Handle(query, CancellationToken.None);
        
        result.IsSuccess.Should().BeFalse();
        _orderRepositoryMock.Verify(repo => repo.GetByUserIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnTrue_WhenOrderIsFound()
    {
        _orderRepositoryMock.Setup(repo => repo.GetByUserIdAsync(It.IsAny<Guid>()))
                            .ReturnsAsync(new Faker<Order>().Generate(3));
        
        var query = new GetOrderByUserIdQuery(Guid.NewGuid());
        var result = await _handler.Handle(query, CancellationToken.None);
        
        result.IsSuccess.Should().BeTrue();
        _orderRepositoryMock.Verify(repo => repo.GetByUserIdAsync(It.IsAny<Guid>()), Times.Once);
    }
}