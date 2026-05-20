using FluentResults;
using MediatR;
using TaskManagement.Application.DTOs.Task;
using TaskManagement.Application.Interfaces;

namespace TaskManagement.Application.Features.Task.Queries;

public class GetTasksByProjectQueryHandler : IRequestHandler<GetTasksByProjectQuery, Result<IEnumerable<TaskDto>>>
{
    private readonly ITaskService _taskService;

    public GetTasksByProjectQueryHandler(ITaskService taskService)
    {
        _taskService = taskService;
    }

    public async Task<Result<IEnumerable<TaskDto>>> Handle(GetTasksByProjectQuery request, CancellationToken cancellationToken)
    {
        var tasks = await _taskService.GetByProjectIdAsync(request.ProjectId, request.UserId);
        return Result.Ok(tasks);
    }
}