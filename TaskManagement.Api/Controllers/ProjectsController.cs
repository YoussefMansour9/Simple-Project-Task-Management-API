using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Common;
using TaskManagement.Application.DTOs.Project;
using TaskManagement.Application.Features.Project.Commands;
using TaskManagement.Application.Features.Project.Queries;
using TaskManagement.Api.Extensions;

namespace TaskManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class ProjectsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProjectsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = this.GetUserId();
        var query = new GetAllProjectsQuery(userId);
        var result = await _mediator.Send(query);

        if (result.IsSuccess)
        {
            return Ok(ApiResponse<IEnumerable<ProjectDto>>.SuccessResponse(
                result.Value,
                "Projects retrieved successfully"));
        }

        return BadRequest(ApiResponse.ErrorResponse("Failed to retrieve projects"));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userId = this.GetUserId();
        var query = new GetProjectByIdQuery(id, userId);
        var result = await _mediator.Send(query);

        if (result.IsSuccess)
        {
            return Ok(ApiResponse<ProjectDto>.SuccessResponse(
                result.Value,
                "Project retrieved successfully"));
        }

        var errors = result.Errors.Select(e => e.Message).ToList();
        return NotFound(ApiResponse<ProjectDto>.ErrorResponse("Project not found", errors));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProjectDto dto)
    {
        var userId = this.GetUserId();
        var command = new CreateProjectCommand(dto, userId);
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Value.Id },
                ApiResponse<ProjectDto>.SuccessResponse(result.Value, "Project created successfully"));
        }

        var errors = result.Errors.Select(e => e.Message).ToList();
        return BadRequest(ApiResponse<ProjectDto>.ErrorResponse("Failed to create project", errors));
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateProjectDto dto)
    {
        var userId = this.GetUserId();
        var command = new UpdateProjectCommand(dto, userId);
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            return Ok(ApiResponse<ProjectDto>.SuccessResponse(
                result.Value,
                "Project updated successfully"));
        }

        var errors = result.Errors.Select(e => e.Message).ToList();
        return BadRequest(ApiResponse<ProjectDto>.ErrorResponse("Failed to update project", errors));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = this.GetUserId();
        var command = new DeleteProjectCommand(id, userId);
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            return Ok(ApiResponse.SuccessResponse("Project deleted successfully"));
        }

        var errors = result.Errors.Select(e => e.Message).ToList();
        return BadRequest(ApiResponse.ErrorResponse("Failed to delete project", errors));
    }
}