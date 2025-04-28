using Microsoft.AspNetCore.Mvc;
using OrderService.MessageBus;

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
}