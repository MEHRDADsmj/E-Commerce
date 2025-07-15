using Bogus;
using FluentAssertions;
using Moq;
using ProductService.Application.Products.Queries.GetProductsPaginated;
using ProductService.Domain.Entities;
using ProductService.Domain.Interfaces;

namespace Tests.ProductService;

public class GetProductsPaginatedHandlerTests
{
    private readonly Mock<IProductRepository> _repositoryMock;
    private readonly GetProductsPaginatedHandler _handler;

    public GetProductsPaginatedHandlerTests()
    {
        _repositoryMock = new Mock<IProductRepository>();
        _handler = new GetProductsPaginatedHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnTrue_WhenProductsAreFound()
    {
        var page = new Faker().Random.Int(1);
        var pageSize = new Faker().Random.Int(1);

        var products = new Faker<Product>().RuleFor(p => p.Id, f => f.Random.Guid())
                                           .RuleFor(p => p.Name, f => f.Random.Word())
                                           .RuleFor(p => p.Description, f => f.Random.Word())
                                           .RuleFor(p => p.UnitPrice, f => f.Random.Decimal())
                                           .Generate(3);

        _repositoryMock
            .Setup(repo => repo.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(products);
        
        var query = new GetProductsPaginatedQuery(page, pageSize);
        var res = await _handler.Handle(query, CancellationToken.None);

        res.IsSuccess.Should().BeTrue();
        _repositoryMock.Verify(repo => repo.GetAllAsync(page, pageSize), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFalse_WhenProductsAreNotFound()
    {
        var page = new Faker().Random.Int(1);
        var pageSize = new Faker().Random.Int(1);
        
        _repositoryMock
            .Setup(repo => repo.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(new List<Product>());
        
        var query = new GetProductsPaginatedQuery(page, pageSize);
        var res = await _handler.Handle(query, CancellationToken.None);
        
        res.IsSuccess.Should().BeFalse();
        _repositoryMock.Verify(repo => repo.GetAllAsync(page, pageSize), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFalse_WhenPageNumberIsOutOfRange()
    {
        var page = new Faker().Random.Int(max: 0);
        var pageSize = new Faker().Random.Int(max: 0);
        
        var query = new GetProductsPaginatedQuery(page, pageSize);
        var res = await _handler.Handle(query, CancellationToken.None);
        
        res.IsSuccess.Should().BeFalse();
        _repositoryMock.Verify(repo => repo.GetAllAsync(page, pageSize), Times.Never);
    }
}