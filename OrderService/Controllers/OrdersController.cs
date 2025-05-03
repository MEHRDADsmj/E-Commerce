using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using OrderService.MessageBus;
using Shared.Messaging;

namespace OrderService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly RabbitMQPublisher _rabbitMQPublisher;
    
    public OrdersController(RabbitMQPublisher rabbitMQPublisher)
    {
        _rabbitMQPublisher = rabbitMQPublisher;
    }

    [HttpGet("health")]
    public IActionResult GetHealth()
    {
        return Ok("Orders Healthy");
    }

    [HttpGet("send-test-message")]
    public async Task<IActionResult> SendTestMessage()
    {
        await _rabbitMQPublisher.PublishMessage("Test Order");
        return Ok("Message sent to RabbitMQ");
    }

    [HttpPost("Order-Create")]
    public async Task PublishOrderCreatedAsync(OrderCreatedEvent orderCreatedEvent)
    {
        var body = JsonSerializer.Serialize(orderCreatedEvent);
        await _rabbitMQPublisher.PublishMessage(body);
    }
}