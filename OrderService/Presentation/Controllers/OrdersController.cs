using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Orders.Commands.CreateOrder;
using OrderService.Application.Orders.Queries.GetOrderById;
using OrderService.Application.Orders.Queries.GetOrderByUserId;
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

    [HttpPut("create")]
    public async Task<ActionResult<CreateOrderResponseDto>> CreateOrder()
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
            var dto = new CreateOrderResponseDto(result.Value);
            return CreatedAtAction("CreateOrder", dto);
        }
        return BadRequest(result.ErrorMessage);
    }

    [HttpGet("get/{id:guid}")]
    public async Task<ActionResult<GetOrderByIdResponseDto>> GetOrderById([FromRoute] Guid id)
    {
        var command = new GetOrderByIdQuery(id);
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            var dto = new GetOrderByIdResponseDto(result.Value!);
            return Ok(dto);
        }
        return BadRequest(result.ErrorMessage);
    }

    [HttpGet("user/{id:guid}")]
    public async Task<ActionResult<GetOrderByUserIdResponseDto>> GetOrdersByUserId([FromRoute] Guid id)
    {
        var command = new GetOrderByUserIdQuery(id);
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            var dto = new GetOrderByUserIdResponseDto(result.Value!);
            return Ok(dto);
        }
        return BadRequest(result.ErrorMessage);
    }
}