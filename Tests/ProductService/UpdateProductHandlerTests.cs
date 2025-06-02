using Bogus;
using FluentAssertions;
using Moq;
using ProductService.Application.Products.Commands.UpdateProduct;
using ProductService.Domain.Entities;
using ProductService.Domain.Interfaces;

namespace Tests.ProductService;

public class UpdateProductHandlerTests
{
    private readonly Mock<IProductRepository> _repositoryMock;
    private readonly UpdateProductHandler _handler;

    public UpdateProductHandlerTests()
    {
        _repositoryMock = new Mock<IProductRepository>();
        _handler = new UpdateProductHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnTrue_WhenProductIsUpdated()
    {
        _repositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Product());
        _repositoryMock
            .Setup(repo => repo.UpdateAsync(It.IsAny<ProductDto>()))
            .ReturnsAsync(new Product());

        var command = new UpdateProductCommand(new ProductDto(Guid.NewGuid(), new Faker().Random.Word(),
                                                              new Faker().Random.Decimal(), new Faker().Random.Word()));
        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        _repositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _repositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<ProductDto>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFalse_WhenProductIsNotUpdated()
    {
        _repositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Product());
        _repositoryMock
            .Setup(repo => repo.UpdateAsync(It.IsAny<ProductDto>()))
            .ThrowsAsync(new Exception("Something went wrong"));

        var command = new UpdateProductCommand(new ProductDto(Guid.NewGuid(), new Faker().Random.Word(),
                                                              new Faker().Random.Decimal(), new Faker().Random.Word()));
        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        _repositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _repositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<ProductDto>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFalse_WhenProductIsNotFound()
    {
        _repositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(null as Product);
        
        var command = new UpdateProductCommand(new ProductDto(Guid.NewGuid(), new Faker().Random.Word(),
                                                              new Faker().Random.Decimal(), new Faker().Random.Word()));
        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        _repositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _repositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<ProductDto>()), Times.Never);
    }
}