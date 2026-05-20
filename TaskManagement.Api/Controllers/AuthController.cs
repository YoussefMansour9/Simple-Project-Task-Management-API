using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Common;
using TaskManagement.Application.DTOs.Auth;
using TaskManagement.Application.Features.Auth.Commands;
using TaskManagement.Application.Features.Auth.Queries;
using TaskManagement.Api.Extensions;

namespace TaskManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[ApiVersion("1.0")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var command = new RegisterCommand(dto);
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            return Ok(ApiResponse<AuthResponseDto>.SuccessResponse(
                result.Value,
                "Registration successful"));
        }

        var errors = result.Errors.Select(e => e.Message).ToList();
        return BadRequest(ApiResponse<AuthResponseDto>.ErrorResponse("Registration failed", errors));
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var command = new LoginCommand(dto);
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            return Ok(ApiResponse<AuthResponseDto>.SuccessResponse(
                result.Value,
                "Login successful"));
        }

        var errors = result.Errors.Select(e => e.Message).ToList();
        return BadRequest(ApiResponse<AuthResponseDto>.ErrorResponse("Login failed", errors));
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userId = this.GetUserId();
        var query = new GetCurrentUserQuery(userId);
        var result = await _mediator.Send(query);

        if (result.IsSuccess)
        {
            return Ok(ApiResponse<object>.SuccessResponse(
                new
                {
                    result.Value.Id,
                    result.Value.FirstName,
                    result.Value.LastName,
                    result.Value.Email,
                    result.Value.Role,
                    result.Value.CreatedAt
                },
                "User retrieved successfully"));
        }

        return BadRequest(ApiResponse.ErrorResponse("Failed to retrieve user"));
    }
}