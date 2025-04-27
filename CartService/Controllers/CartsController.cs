using Microsoft.AspNetCore.Mvc;

namespace CartService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CartsController : ControllerBase
{
    public CartsController()
    {
        
    }

    [HttpGet("health")]
    public IActionResult GetHealth()
    {
        return Ok("Carts Healthy");
    }
}