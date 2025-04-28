using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace CartService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CartsController : ControllerBase
{
    private readonly IConnectionMultiplexer _redis;
    
    public CartsController(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    [HttpGet("health")]
    public IActionResult GetHealth()
    {
        return Ok("Carts Healthy");
    }

    [HttpGet("ping")]
    public async Task<IActionResult> PingRedis()
    {
        var db = _redis.GetDatabase();
        var pong = await db.PingAsync();
        return Ok($"Redis responded in {pong.TotalMilliseconds} ms");
    }
}