using CartService.Application.Carts.Commands.UpdateItemQuantity;
using CartService.Domain.Entities;
using CartService.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace Tests.CartService;

public class UpdateItemQuantityHandlerTests
{
    private readonly Mock<ICartRepository> _cartRepositoryMock;
    private readonly UpdateItemQuantityHandler _handler;

    public UpdateItemQuantityHandlerTests()
    {
        _cartRepositoryMock = new Mock<ICartRepository>();
        _handler = new UpdateItemQuantityHandler(_cartRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnTrue_WhenItemIsUpdated()
    {
        _cartRepositoryMock
            .Setup(repo => repo.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Cart());
        _cartRepositoryMock
            .Setup(repo => repo.UpdateItemQuantityAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<int>()))
            .Returns(Task.CompletedTask);
        
        var rnd = new Random();
        var command = new UpdateItemQuantityCommand(Guid.NewGuid(), Guid.NewGuid(), rnd.Next(1, int.MaxValue));
        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        _cartRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<Guid>()), Times.Once);
        _cartRepositoryMock.Verify(
            repo => repo.UpdateItemQuantityAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<int>()), Times.Once);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnFalse_IncorrectItemQuantity()
    {
        _cartRepositoryMock
            .Setup(repo => repo.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Cart());
        _cartRepositoryMock
            .Setup(repo => repo.UpdateItemQuantityAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<int>()))
            .ThrowsAsync(new ArgumentOutOfRangeException("item quantity"));
        
        var rnd = new Random();
        var command = new UpdateItemQuantityCommand(Guid.NewGuid(), Guid.NewGuid(), rnd.Next(int.MinValue, 0));
        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        _cartRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<Guid>()), Times.Once);
        _cartRepositoryMock.Verify(
            repo => repo.UpdateItemQuantityAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<int>()), Times.Once);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnFalse_CartNotFound()
    {
        _cartRepositoryMock
            .Setup(repo => repo.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync(null as Cart);
        
        var rnd = new Random();
        var command = new UpdateItemQuantityCommand(Guid.NewGuid(), Guid.NewGuid(), rnd.Next(int.MinValue, 0));
        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        _cartRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<Guid>()), Times.Once);
        _cartRepositoryMock.Verify(
            repo => repo.UpdateItemQuantityAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<int>()), Times.Never);
    }
}