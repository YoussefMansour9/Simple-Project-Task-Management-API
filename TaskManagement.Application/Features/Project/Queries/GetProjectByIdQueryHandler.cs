using FluentResults;
using MediatR;
using TaskManagement.Application.DTOs.Project;
using TaskManagement.Application.Interfaces;

namespace TaskManagement.Application.Features.Project.Queries;

public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, Result<ProjectDto>>
{
    private readonly IProjectService _projectService;

    public GetProjectByIdQueryHandler(IProjectService projectService)
    {
        _projectService = projectService;
    }

    public async Task<Result<ProjectDto>> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var project = await _projectService.GetByIdAsync(request.Id, request.UserId);
        if (project == null)
        {
            return Result.Fail("Project not found or access denied");
        }
        return Result.Ok(project);
    }
}