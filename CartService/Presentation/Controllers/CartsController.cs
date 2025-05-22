using CartService.Application.Carts.Commands.AddItemToCart;
using CartService.Application.Carts.Commands.GetCart;
using CartService.Presentation.DTOs;
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
    
    private bool GetUserIdFromClaims(out string? userId, out IActionResult actionResult)
    {
        userId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "user_id")?.Value;
        if (userId == null)
        {
            actionResult = Unauthorized("JWT token is invalid");
            return true;
        }

        actionResult = new OkResult();
        return false;
    }

    [HttpGet]
    public async Task<IActionResult> GetCart()
    {
        if (GetUserIdFromClaims(out var userId, out var actionResult)) return actionResult;

        var query = new GetCartQuery(Guid.Parse(userId));
        var result = await _mediator.Send(query);

        if (result.IsSuccess)
        {
            var resp = new GetUserCartResponseDto(result.Value);
            return Ok(resp);
        }
        return BadRequest(result.ErrorMessage);
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddItemToCart(AddItemToCartRequestDto dto)
    {
        if (GetUserIdFromClaims(out var userId, out var actionResult)) return actionResult;

        var command = new AddItemToCartCommand(Guid.Parse(userId), dto.ProductId, dto.Quantity);
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            var resp = new AddItemToCartResponseDto(dto.ProductId, dto.Quantity);
            return Ok(resp);
        }
        return BadRequest(result.ErrorMessage);
    }
}