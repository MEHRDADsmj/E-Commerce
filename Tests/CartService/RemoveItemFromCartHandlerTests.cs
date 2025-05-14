using CartService.Application.Carts.Commands.RemoveItemFromCart;
using CartService.Domain.Entities;
using CartService.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace Tests.CartService;

public class RemoveItemFromCartHandlerTests
{
    private readonly Mock<ICartRepository> _cartRepositoryMock;
    private readonly RemoveItemFromCartHandler _handler;

    public RemoveItemFromCartHandlerTests()
    {
        _cartRepositoryMock = new Mock<ICartRepository>();
        _handler = new RemoveItemFromCartHandler(_cartRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnTrue_WhenItemIsRemoved()
    {
        _cartRepositoryMock
            .Setup(repo => repo.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Cart());
        _cartRepositoryMock
            .Setup(repo => repo.RemoveItemAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .Returns(Task.CompletedTask);

        var command = new RemoveItemFromCartCommand(Guid.NewGuid(), Guid.NewGuid());
        var result = await _handler.Handle(command, CancellationToken.None);
        
        result.IsSuccess.Should().BeTrue();
        _cartRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<Guid>()), Times.Once);
        _cartRepositoryMock.Verify(repo => repo.RemoveItemAsync(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Once);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnFalse_WhenCartNotFound()
    {
        _cartRepositoryMock
            .Setup(repo => repo.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync(null as Cart);

        var command = new RemoveItemFromCartCommand(Guid.NewGuid(), Guid.NewGuid());
        var result = await _handler.Handle(command, CancellationToken.None);
        
        result.IsSuccess.Should().BeFalse();
        _cartRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<Guid>()), Times.Once);
        _cartRepositoryMock.Verify(repo => repo.RemoveItemAsync(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
    }
}