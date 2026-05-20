using FluentResults;
using MediatR;
using TaskManagement.Application.Interfaces;

namespace TaskManagement.Application.Features.Project.Commands;

public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, Result<bool>>
{
    private readonly IProjectService _projectService;

    public DeleteProjectCommandHandler(IProjectService projectService)
    {
        _projectService = projectService;
    }

    public async Task<Result<bool>> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var result = await _projectService.DeleteAsync(request.Id, request.UserId);
        if (!result)
        {
            return Result.Fail("Project not found or access denied");
        }
        return Result.Ok(true);
    }
}