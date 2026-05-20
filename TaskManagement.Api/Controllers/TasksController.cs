using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Common;
using TaskManagement.Application.DTOs.Task;
using TaskManagement.Application.Features.Task.Commands;
using TaskManagement.Application.Features.Task.Queries;
using TaskManagement.Api.Extensions;

namespace TaskManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly IMediator _mediator;

    public TasksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("project/{projectId:guid}")]
    public async Task<IActionResult> GetByProject(Guid projectId)
    {
        var userId = this.GetUserId();
        var query = new GetTasksByProjectQuery(projectId, userId);
        var result = await _mediator.Send(query);

        if (result.IsSuccess)
        {
            return Ok(ApiResponse<IEnumerable<TaskDto>>.SuccessResponse(
                result.Value,
                "Tasks retrieved successfully"));
        }

        return BadRequest(ApiResponse.ErrorResponse("Failed to retrieve tasks"));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userId = this.GetUserId();
        var query = new GetTaskByIdQuery(id, userId);
        var result = await _mediator.Send(query);

        if (result.IsSuccess)
        {
            return Ok(ApiResponse<TaskDto>.SuccessResponse(
                result.Value,
                "Task retrieved successfully"));
        }

        var errors = result.Errors.Select(e => e.Message).ToList();
        return NotFound(ApiResponse<TaskDto>.ErrorResponse("Task not found", errors));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskDto dto)
    {
        var userId = this.GetUserId();
        var command = new CreateTaskCommand(dto, userId);
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Value.Id },
                ApiResponse<TaskDto>.SuccessResponse(result.Value, "Task created successfully"));
        }

        var errors = result.Errors.Select(e => e.Message).ToList();
        return BadRequest(ApiResponse<TaskDto>.ErrorResponse("Failed to create task", errors));
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateTaskDto dto)
    {
        var userId = this.GetUserId();
        var command = new UpdateTaskCommand(dto, userId);
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            return Ok(ApiResponse<TaskDto>.SuccessResponse(
                result.Value,
                "Task updated successfully"));
        }

        var errors = result.Errors.Select(e => e.Message).ToList();
        return BadRequest(ApiResponse<TaskDto>.ErrorResponse("Failed to update task", errors));
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateTaskStatusDto dto)
    {
        var userId = this.GetUserId();
        dto.Id = id;
        var command = new UpdateTaskStatusCommand(id, dto.Status, userId);
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            return Ok(ApiResponse<TaskDto>.SuccessResponse(
                result.Value,
                "Task status updated successfully"));
        }

        var errors = result.Errors.Select(e => e.Message).ToList();
        return BadRequest(ApiResponse<TaskDto>.ErrorResponse("Failed to update task status", errors));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = this.GetUserId();
        var command = new DeleteTaskCommand(id, userId);
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            return Ok(ApiResponse.SuccessResponse("Task deleted successfully"));
        }

        var errors = result.Errors.Select(e => e.Message).ToList();
        return BadRequest(ApiResponse.ErrorResponse("Failed to delete task", errors));
    }
}