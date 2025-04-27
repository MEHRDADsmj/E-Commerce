using Microsoft.AspNetCore.Mvc;

namespace UserService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    public UsersController()
    {
        
    }

    [HttpGet("health")]
    public IActionResult GetHealth()
    {
        return Ok("Users Healthy");
    }
}