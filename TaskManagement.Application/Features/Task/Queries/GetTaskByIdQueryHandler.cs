using FluentResults;
using MediatR;
using TaskManagement.Application.DTOs.Task;
using TaskManagement.Application.Interfaces;

namespace TaskManagement.Application.Features.Task.Queries;

public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, Result<TaskDto>>
{
    private readonly ITaskService _taskService;

    public GetTaskByIdQueryHandler(ITaskService taskService)
    {
        _taskService = taskService;
    }

    public async Task<Result<TaskDto>> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        var task = await _taskService.GetByIdAsync(request.Id, request.UserId);
        if (task == null)
        {
            return Result.Fail("Task not found or access denied");
        }
        return Result.Ok(task);
    }
}