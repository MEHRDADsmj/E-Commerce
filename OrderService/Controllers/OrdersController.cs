using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Interfaces;

namespace OrderService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IEventPublisher _eventPublisher;
    
    public OrdersController(IEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;
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