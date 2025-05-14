using CartService.Application.Carts.Commands.ClearCart;
using CartService.Domain.Entities;
using CartService.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace Tests.CartService;

public class ClearCartHandlerTests
{
    private readonly Mock<ICartRepository> _cartRepositoryMock;
    private readonly ClearCartHandler _handler;

    public ClearCartHandlerTests()
    {
        _cartRepositoryMock = new Mock<ICartRepository>();
        _handler = new ClearCartHandler(_cartRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnTrue_WhenCartCleared()
    {
        _cartRepositoryMock
            .Setup(repo => repo.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Cart());
        _cartRepositoryMock
            .Setup(repo => repo.ClearCartAsync(It.IsAny<Guid>()))
            .Returns(Task.CompletedTask);
        
        var command = new ClearCartCommand(Guid.NewGuid());
        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        _cartRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<Guid>()), Times.Once);
        _cartRepositoryMock.Verify(repo => repo.ClearCartAsync(It.IsAny<Guid>()), Times.Once);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnTrue_WhenCartNotFound()
    {
        _cartRepositoryMock
            .Setup(repo => repo.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync(null as Cart);
        
        var command = new ClearCartCommand(Guid.NewGuid());
        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        _cartRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<Guid>()), Times.Once);
        _cartRepositoryMock.Verify(repo => repo.ClearCartAsync(It.IsAny<Guid>()), Times.Never);
    }
}