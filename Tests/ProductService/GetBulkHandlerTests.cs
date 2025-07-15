using Bogus;
using FluentAssertions;
using Moq;
using ProductService.Application.Products.Queries.GetBulk;
using ProductService.Domain.Entities;
using ProductService.Domain.Interfaces;

namespace Tests.ProductService;

public class GetBulkHandlerTests
{
    private readonly Mock<IProductRepository> _repositoryMock;
    private readonly GetProductsBulkHandler _handler;

    public GetBulkHandlerTests()
    {
        _repositoryMock = new Mock<IProductRepository>();
        _handler = new GetProductsBulkHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnTrue_WhenProductsExists()
    {
        List<Product> fakeProducts = new Faker<Product>()
                                     .RuleFor(p => p.Id, f => f.Random.Guid())
                                     .RuleFor(p => p.Name, f => f.Random.Word())
                                     .RuleFor(p => p.Description, f => f.Random.Word())
                                     .RuleFor(p => p.UnitPrice, f => f.Random.Decimal())
                                     .Generate(10);
        
        _repositoryMock
            .Setup(repo => repo.GetBulkAsync(It.IsAny<IEnumerable<Guid>>()))
            .ReturnsAsync(fakeProducts);

        var list = new List<Guid>();
        for (int i = 0; i < 10; i++)
        {
            list.Add(new Faker().Random.Guid());
        }
        var query = new GetProductsBulkQuery(list);
        var res = await _handler.Handle(query, CancellationToken.None);

        res.IsSuccess.Should().BeTrue();
        res.Value.Should().BeEquivalentTo(fakeProducts);
        _repositoryMock.Verify(repo => repo.GetBulkAsync(It.IsAny<IEnumerable<Guid>>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFalse_WhenIDsDoNotExist()
    {
        var query = new GetProductsBulkQuery(new List<Guid>());
        var res = await _handler.Handle(query, CancellationToken.None);
        
        res.IsSuccess.Should().BeFalse();
        _repositoryMock.Verify(repo => repo.GetBulkAsync(It.IsAny<IEnumerable<Guid>>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnFalse_WhenProductsDoNotExist()
    {
        _repositoryMock
            .Setup(repo => repo.GetBulkAsync(It.IsAny<IEnumerable<Guid>>()))
            .ReturnsAsync(new List<Product>());

        var list = new List<Guid>();
        for (int i = 0; i < 10; i++)
        {
            list.Add(new Faker().Random.Guid());
        }
        var query = new GetProductsBulkQuery(list);
        var res = await _handler.Handle(query, CancellationToken.None);

        res.IsSuccess.Should().BeFalse();
        _repositoryMock.Verify(repo => repo.GetBulkAsync(It.IsAny<IEnumerable<Guid>>()), Times.Once);
    }
}