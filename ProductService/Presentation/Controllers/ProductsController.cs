using Microsoft.AspNetCore.Mvc;

namespace ProductService.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    public ProductsController()
    {
        
    }

    [HttpGet("health")]
    public IActionResult GetHealth()
    {
        return Ok("Products Healthy");
    }
}