using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Users.Commands.GetUserProfile;
using UserService.Application.Users.Commands.LoginUser;

namespace UserService.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [AllowAnonymous]
    [HttpGet("health")]
    public IActionResult GetHealth()
    {
        return Ok("Users Healthy");
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserProfile(Guid userId)
    {
        var command = new GetUserProfileQuery(userId);
        var result = await _mediator.Send(command);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.ErrorMessage);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserCommand command)
    {
        var result = await _mediator.Send(command);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.ErrorMessage);
    }
}