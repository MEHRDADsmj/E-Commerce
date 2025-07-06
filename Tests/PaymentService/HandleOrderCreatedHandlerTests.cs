using Bogus;
using FluentAssertions;
using Moq;
using PaymentService.Application.Interfaces;
using PaymentService.Application.Payments.Commands.HandleOrderCreated;
using PaymentService.Contracts.Events;
using PaymentService.Domain.Entities;
using PaymentService.Domain.Interfaces;

namespace Tests.PaymentService;

public class HandleOrderCreatedHandlerTests
{
    private readonly HandleOrderCreatedHandler _handler;
    private readonly Mock<IEventPublisher> _eventPublisherMock;
    private readonly Mock<IPaymentProcessor> _paymentProcessorMock;

    public HandleOrderCreatedHandlerTests()
    {
        _eventPublisherMock = new Mock<IEventPublisher>();
        _paymentProcessorMock = new Mock<IPaymentProcessor>();
        _handler = new HandleOrderCreatedHandler(_paymentProcessorMock.Object, _eventPublisherMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnTrue_WhenPaymentIsProcessed()
    {
        var payment = new Payment(Guid.NewGuid(), Guid.NewGuid(), new Faker().Random.Decimal());
        payment.MarkAsCompleted();
        _paymentProcessorMock.Setup(payment => payment.ProcessAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<decimal>()))
                             .ReturnsAsync(payment);
        
        _eventPublisherMock.Setup(publisher => publisher.PublishAsync(It.IsAny<PaymentCompletedEvent>(), It.IsAny<string>()))
                           .Returns(Task.CompletedTask);
        
        var command = new HandleOrderCreatedCommand(Guid.NewGuid(), Guid.NewGuid(), new Faker().Random.Decimal());
        var res = await _handler.Handle(command, CancellationToken.None);

        res.IsSuccess.Should().BeTrue();
        _paymentProcessorMock.Verify(processor => processor.ProcessAsync(It.IsAny<Guid>(), 
                                                                         It.IsAny<Guid>(), It.IsAny<decimal>()), Times.Once);
        _eventPublisherMock.Verify(publisher => publisher.PublishAsync(It.IsAny<IEvent>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFalse_WhenPaymentIsNotProcessed()
    {
        var payment = new Payment(Guid.NewGuid(), Guid.NewGuid(), new Faker().Random.Decimal());
        payment.MarkAsFailed();
        _paymentProcessorMock.Setup(payment => payment.ProcessAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<decimal>()))
                             .ReturnsAsync(payment);
        
        _eventPublisherMock.Setup(publisher => publisher.PublishAsync(It.IsAny<PaymentCompletedEvent>(), It.IsAny<string>()))
                           .Returns(Task.CompletedTask);
        
        var command = new HandleOrderCreatedCommand(Guid.NewGuid(), Guid.NewGuid(), new Faker().Random.Decimal());
        var res = await _handler.Handle(command, CancellationToken.None);

        res.IsSuccess.Should().BeFalse();
        _paymentProcessorMock.Verify(processor => processor.ProcessAsync(It.IsAny<Guid>(), 
                                                                         It.IsAny<Guid>(), It.IsAny<decimal>()), Times.Once);
        _eventPublisherMock.Verify(publisher => publisher.PublishAsync(It.IsAny<IEvent>(), It.IsAny<string>()), Times.Once);
    }
}