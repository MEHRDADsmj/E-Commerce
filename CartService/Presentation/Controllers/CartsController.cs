using CartService.Application.Carts.Commands.AddItemToCart;
using CartService.Application.Carts.Commands.GetCart;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CartService.Presentation.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class CartsController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public CartsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [AllowAnonymous]
    [HttpGet("health")]
    public IActionResult GetHealth()
    {
        return Ok("Carts Healthy");
    }

    [HttpGet]
    public async Task<IActionResult> GetCart()
    {
        var userId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "user_id")?.Value;
        if (userId == null)
        {
            return Unauthorized("JWT token is invalid");
        }

        var query = new GetCartQuery(Guid.Parse(userId));
        var result = await _mediator.Send(query);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.ErrorMessage);
    }
}