using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Orders.Commands.CreateOrder;
using OrderService.Application.Orders.Commands.GetOrderById;
using OrderService.Application.Orders.Commands.GetOrderByUserId;
using OrderService.Presentation.DTOs;

namespace OrderService.Presentation.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [AllowAnonymous]
    [HttpGet("health")]
    public IActionResult GetHealth()
    {
        return Ok("Orders Healthy");
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateOrder()
    {
        var userId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "user_id")?.Value;
        if (userId == null)
        {
            return Unauthorized();
        }

        string jwtToken = HttpContext.Request.Headers.Authorization.ToString().Split()[1];
        var command = new CreateOrderCommand(Guid.Parse(userId), jwtToken);
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.ErrorMessage);
    }

    [HttpGet("get/{id}")]
    public async Task<IActionResult> GetOrderById(Guid id)
    {
        var command = new GetOrderByIdQuery(id);
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            var dto = new GetOrderByIdResponseDto(result.Value);
            return Ok(dto);
        }
        return BadRequest(result.ErrorMessage);
    }

    [HttpGet("user/{id}")]
    public async Task<IActionResult> GetOrdersByUserId(Guid id)
    {
        var command = new GetOrderByUserIdQuery(id);
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            var dto = new GetOrderByUserIdResponseDto(result.Value);
            return Ok(dto);
        }
        return BadRequest(result.ErrorMessage);
    }
}