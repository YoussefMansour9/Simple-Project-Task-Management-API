using FluentResults;
using MediatR;
using TaskManagement.Application.Interfaces;

namespace TaskManagement.Application.Features.Task.Commands;

public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, Result<bool>>
{
    private readonly ITaskService _taskService;

    public DeleteTaskCommandHandler(ITaskService taskService)
    {
        _taskService = taskService;
    }

    public async Task<Result<bool>> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        var result = await _taskService.DeleteAsync(request.Id, request.UserId);
        if (!result)
        {
            return Result.Fail("Task not found or access denied");
        }
        return Result.Ok(true);
    }
}