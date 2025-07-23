using CartService.Application.Carts.Queries.GetCart;
using CartService.Domain.Entities;
using CartService.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace Tests.CartService;

public class GetCartHandlerTests
{
    private readonly Mock<ICartRepository> _cartRepositoryMock;
    private readonly GetCartHandler _handler;

    public GetCartHandlerTests()
    {
        _cartRepositoryMock = new Mock<ICartRepository>();
        _handler = new GetCartHandler(_cartRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnCart_WhenCartExists()
    {
        _cartRepositoryMock
            .Setup(cartRepo => cartRepo.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Cart(Guid.NewGuid(), new List<CartItem>()));

        var query = new GetCartQuery(Guid.NewGuid());
        var res = await _handler.Handle(query, CancellationToken.None);
        res.IsSuccess.Should().BeTrue();
        res.Value.Should().NotBeNull();
        res.Value.Should().BeOfType<Cart>();
        _cartRepositoryMock.Verify(cartRepo => cartRepo.GetAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldNotReturnCart_WhenCartDoesNotExist()
    {
        _cartRepositoryMock
            .Setup(cartRepo => cartRepo.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync(Cart.Empty);
        
        var query = new GetCartQuery(Guid.NewGuid());
        var res = await _handler.Handle(query, CancellationToken.None);
        res.IsSuccess.Should().BeFalse();
        res.Value.Should().BeNull();
        _cartRepositoryMock.Verify(cartRepo => cartRepo.GetAsync(It.IsAny<Guid>()), Times.Once);
    }
}