using FluentResults;
using MediatR;
using TaskManagement.Application.DTOs.Project;
using TaskManagement.Application.Interfaces;

namespace TaskManagement.Application.Features.Project.Commands;

public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, Result<ProjectDto>>
{
    private readonly IProjectService _projectService;

    public UpdateProjectCommandHandler(IProjectService projectService)
    {
        _projectService = projectService;
    }

    public async Task<Result<ProjectDto>> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var result = await _projectService.UpdateAsync(request.Dto, request.UserId);
        if (result == null)
        {
            return Result.Fail("Project not found or access denied");
        }
        return Result.Ok(result);
    }
}