using Microsoft.AspNetCore.Mvc;

namespace PaymentService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentsController : ControllerBase
{
    public PaymentsController()
    {
        
    }

    [HttpGet("health")]
    public IActionResult GetHealth()
    {
        return Ok("Payments Healthy");
    }
}