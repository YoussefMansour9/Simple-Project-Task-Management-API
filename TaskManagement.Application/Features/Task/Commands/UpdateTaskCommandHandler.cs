using FluentResults;
using MediatR;
using TaskManagement.Application.DTOs.Task;
using TaskManagement.Application.Interfaces;

namespace TaskManagement.Application.Features.Task.Commands;

public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, Result<TaskDto>>
{
    private readonly ITaskService _taskService;

    public UpdateTaskCommandHandler(ITaskService taskService)
    {
        _taskService = taskService;
    }

    public async Task<Result<TaskDto>> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var result = await _taskService.UpdateAsync(request.Dto, request.UserId);
        if (result == null)
        {
            return Result.Fail("Task not found or access denied");
        }
        return Result.Ok(result);
    }
}