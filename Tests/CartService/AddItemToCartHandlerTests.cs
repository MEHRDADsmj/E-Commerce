using CartService.Application.Carts.Commands.AddItemToCart;
using CartService.Domain.Entities;
using CartService.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace Tests.CartService;

public class AddItemToCartHandlerTests
{
    private readonly Mock<ICartRepository> _cartRepositoryMock;
    private readonly AddItemToCartHandler _handler;

    public AddItemToCartHandlerTests()
    {
        _cartRepositoryMock = new Mock<ICartRepository>();
        _handler = new AddItemToCartHandler(_cartRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnTrue_WhenItemIsAddedToCart_CartExists()
    {
        _cartRepositoryMock
            .Setup(cartRepo => cartRepo.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Cart());
        _cartRepositoryMock
            .Setup(cartRepo => cartRepo.AddItemAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<int>()))
            .Returns(Task.CompletedTask);
            
        var rnd = new Random();
        var command = new AddItemToCartCommand(Guid.NewGuid(), Guid.NewGuid(), rnd.Next(1, int.MaxValue));
        var result = await _handler.Handle(command, CancellationToken.None);
        
        result.IsSuccess.Should().BeTrue();
        _cartRepositoryMock.Verify(cartRepo => cartRepo.GetAsync(It.IsAny<Guid>()), Times.Once);
        _cartRepositoryMock.Verify(cartRepo => cartRepo.AddItemAsync(
                                       It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<int>()), Times.Once);
        _cartRepositoryMock.Verify(cartRepo => cartRepo.CreateCartAsync(It.IsAny<Guid>()), Times.Never);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnTrue_WhenItemIsAddedToCart_CartDoesNotExists()
    {
        _cartRepositoryMock
            .Setup(cartRepo => cartRepo.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync(null as Cart);
        _cartRepositoryMock
            .Setup(cartRepo => cartRepo.CreateCartAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Cart());
        _cartRepositoryMock
            .Setup(cartRepo => cartRepo.AddItemAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<int>()))
            .Returns(Task.CompletedTask);
            
        var rnd = new Random();
        var command = new AddItemToCartCommand(Guid.NewGuid(), Guid.NewGuid(), rnd.Next(1, int.MaxValue));
        var result = await _handler.Handle(command, CancellationToken.None);
        
        result.IsSuccess.Should().BeTrue();
        _cartRepositoryMock.Verify(cartRepo => cartRepo.GetAsync(It.IsAny<Guid>()), Times.Once);
        _cartRepositoryMock.Verify(cartRepo => cartRepo.AddItemAsync(
                                       It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<int>()), Times.Once);
        _cartRepositoryMock.Verify(cartRepo => cartRepo.CreateCartAsync(It.IsAny<Guid>()), Times.AtMostOnce);
    }

    [Fact]
    public async Task Handle_ShouldReturnFalse_WhenItemIsAddedToCart_CartNotCreated()
    {
        _cartRepositoryMock
            .Setup(cartRepo => cartRepo.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync(null as Cart);
        _cartRepositoryMock
            .Setup(cartRepo => cartRepo.CreateCartAsync(It.IsAny<Guid>()))
            .ReturnsAsync(null as Cart);
            
        var rnd = new Random();
        var command = new AddItemToCartCommand(Guid.NewGuid(), Guid.NewGuid(), rnd.Next(1, int.MaxValue));
        var result = await _handler.Handle(command, CancellationToken.None);
        
        result.IsSuccess.Should().BeFalse();
        _cartRepositoryMock.Verify(cartRepo => cartRepo.GetAsync(It.IsAny<Guid>()), Times.Once);
        _cartRepositoryMock.Verify(cartRepo => cartRepo.AddItemAsync(
                                       It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<int>()), Times.Never);
        _cartRepositoryMock.Verify(cartRepo => cartRepo.CreateCartAsync(It.IsAny<Guid>()), Times.AtMostOnce);
    }

    [Fact]
    public async Task Handle_ShouldReturnFalse_IncorrectItemQuantity()
    {
        _cartRepositoryMock
            .Setup(cartRepo => cartRepo.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Cart());
        _cartRepositoryMock
            .Setup(cartRepo => cartRepo.AddItemAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<int>()))
            .ThrowsAsync(new ArgumentOutOfRangeException("Quantity"));
            
        var rnd = new Random();
        var command = new AddItemToCartCommand(Guid.NewGuid(), Guid.NewGuid(), rnd.Next(Int32.MinValue, 0));
        var result = await _handler.Handle(command, CancellationToken.None);
        
        result.IsSuccess.Should().BeFalse();
        _cartRepositoryMock.Verify(cartRepo => cartRepo.GetAsync(It.IsAny<Guid>()), Times.Once);
        _cartRepositoryMock.Verify(cartRepo => cartRepo.AddItemAsync(
                                       It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<int>()), Times.Once);
        _cartRepositoryMock.Verify(cartRepo => cartRepo.CreateCartAsync(It.IsAny<Guid>()), Times.AtMostOnce);
    }
}