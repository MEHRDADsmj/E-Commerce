using FluentAssertions;
using Moq;
using ProductService.Application.Products.Queries.GetProductById;
using ProductService.Domain.Entities;
using ProductService.Domain.Interfaces;

namespace Tests.ProductService;

public class GetProductByIdHandlerTests
{
    private readonly Mock<IProductRepository> _repositoryMock;
    private readonly GetProductByIdHandler _handler;

    public GetProductByIdHandlerTests()
    {
        _repositoryMock = new Mock<IProductRepository>();
        _handler = new GetProductByIdHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnTrue_WhenProductExists()
    {
        _repositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Product());

        var query = new GetProductByIdQuery(Guid.NewGuid());
        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        _repositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFalse_WhenProductDoesNotExist()
    {
        _repositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(Product.Empty);
        
        var query = new GetProductByIdQuery(Guid.NewGuid());
        var result = await _handler.Handle(query, CancellationToken.None);
        
        result.IsSuccess.Should().BeFalse();
        _repositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }
}