using FluentAssertions;
using Moq;
using ProductService.Application.Products.Commands.DeleteProduct;
using ProductService.Domain.Interfaces;

namespace Tests.ProductService;

public class DeleteProductHandlerTests
{
    private readonly Mock<IProductRepository> _repositoryMock;
    private readonly DeleteProductHandler _handler;

    public DeleteProductHandlerTests()
    {
        _repositoryMock = new Mock<IProductRepository>();
        _handler = new DeleteProductHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnTrue_WhenProductIsDeleted()
    {
        _repositoryMock
            .Setup(repo => repo.DeleteAsync(It.IsAny<Guid>()))
            .Returns(Task.CompletedTask);
        
        var command = new DeleteProductCommand(Guid.NewGuid());
        var res = await _handler.Handle(command, CancellationToken.None);

        res.IsSuccess.Should().BeTrue();
        _repositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFalse_WhenProductIsNotDeleted()
    {
        _repositoryMock
            .Setup(repo => repo.DeleteAsync(It.IsAny<Guid>()))
            .Throws(new Exception("Something went wrong"));
        
        var command = new DeleteProductCommand(Guid.NewGuid());
        var res = await _handler.Handle(command, CancellationToken.None);

        res.IsSuccess.Should().BeFalse();
        _repositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<Guid>()), Times.Once);
    }
}