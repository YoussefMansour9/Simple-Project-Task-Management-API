using FluentResults;
using MediatR;
using TaskManagement.Application.DTOs.Task;
using TaskManagement.Application.Interfaces;

namespace TaskManagement.Application.Features.Task.Commands;

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, Result<TaskDto>>
{
    private readonly ITaskService _taskService;

    public CreateTaskCommandHandler(ITaskService taskService)
    {
        _taskService = taskService;
    }

    public async Task<Result<TaskDto>> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var result = await _taskService.CreateAsync(request.Dto, request.UserId);
        if (result == null)
        {
            return Result.Fail("Failed to create task");
        }
        return Result.Ok(result);
    }
}