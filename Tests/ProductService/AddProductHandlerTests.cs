using Bogus;
using FluentAssertions;
using Moq;
using ProductService.Application.Products.Commands.AddProduct;
using ProductService.Domain.Entities;
using ProductService.Domain.Interfaces;

namespace Tests.ProductService;

public class AddProductHandlerTests
{
    private readonly Mock<IProductRepository> _repositoryMock;
    private readonly AddProductHandler _handler;

    public AddProductHandlerTests()
    {
        _repositoryMock = new Mock<IProductRepository>();
        _handler = new AddProductHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnTrue_WhenProductIsAdded()
    {
        _repositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<Product>()))
            .Returns(Task.CompletedTask);

        var command = new AddProductCommand(new Faker().Random.String(), new Faker().Random.Decimal(),
                                            new Faker().Random.String());
        var res = await _handler.Handle(command, CancellationToken.None);

        res.IsSuccess.Should().BeTrue();
        _repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Product>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFalse_WhenProductIsNotAdded()
    {
        _repositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<Product>()))
            .ThrowsAsync(new Exception("Something went wrong"));
        
        var command = new AddProductCommand(new Faker().Random.String(), new Faker().Random.Decimal(),
                                            new Faker().Random.String());
        var res = await _handler.Handle(command, CancellationToken.None);
        
        res.IsSuccess.Should().BeFalse();
        _repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Product>()), Times.Once);
    }
}