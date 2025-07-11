﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Users.Commands.GetUserProfile;
using UserService.Application.Users.Commands.LoginUser;
using UserService.Application.Users.Commands.RegisterUser;
using UserService.Presentation.DTOs;

namespace UserService.Presentation.Controllers;

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

    [HttpGet("user/{userId:guid}")]
    public async Task<IActionResult> GetUserProfile(Guid userId)
    {
        var command = new GetUserProfileQuery(userId);
        var result = await _mediator.Send(command);
        if (result.IsSuccess)
        {
            var resp = new GetUserProfileResponseDto(result.Value.Email, result.Value.FullName);
            return Ok(resp);
        }
        return BadRequest(result.ErrorMessage);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserRequestDto dto)
    {
        var command = new LoginUserCommand(dto.Email, dto.Password);
        var result = await _mediator.Send(command);
        if (result.IsSuccess)
        {
            var resp = new LoginUserResponseDto(result.Value.Id, result.Value.Email, result.Value.Token);
            return Ok(resp);
        }
        return BadRequest(result.ErrorMessage);
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserRequestDto dto)
    {
        var command = new RegisterUserCommand(dto.Email, dto.Password, dto.FullName);
        var result = await _mediator.Send(command);
        if (result.IsSuccess)
        {
            var resp = new RegisterUserResponseDto(result.Value, dto.Email);
            return Ok(resp);
        }
        return BadRequest(result.ErrorMessage);
    }
}