using Microsoft.AspNetCore.Mvc;
using OrderService.Infrastructure.Messaging;

namespace OrderService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly RabbitMqPublisher _rabbitMqPublisher;
    
    public OrdersController(RabbitMqPublisher rabbitMqPublisher)
    {
        _rabbitMqPublisher = rabbitMqPublisher;
    }

    [HttpGet("health")]
    public IActionResult GetHealth()
    {
        return Ok("Orders Healthy");
    }

    [HttpGet("send-test-message")]
    public async Task<IActionResult> SendTestMessage()
    {
        return Ok("Message sent to RabbitMQ");
    }
}