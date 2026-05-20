using FluentResults;
using MediatR;
using TaskManagement.Application.DTOs.Task;
using TaskManagement.Application.Interfaces;

namespace TaskManagement.Application.Features.Task.Commands;

public class UpdateTaskStatusCommandHandler : IRequestHandler<UpdateTaskStatusCommand, Result<TaskDto>>
{
    private readonly ITaskService _taskService;

    public UpdateTaskStatusCommandHandler(ITaskService taskService)
    {
        _taskService = taskService;
    }

    public async Task<Result<TaskDto>> Handle(UpdateTaskStatusCommand request, CancellationToken cancellationToken)
    {
        var result = await _taskService.UpdateStatusAsync(request.Id, request.Status, request.UserId);
        if (result == null)
        {
            return Result.Fail("Task not found or access denied");
        }
        return Result.Ok(result);
    }
}