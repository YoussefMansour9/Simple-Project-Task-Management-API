using FluentResults;
using MediatR;
using TaskManagement.Application.DTOs.Project;
using TaskManagement.Application.Interfaces;

namespace TaskManagement.Application.Features.Project.Queries;

public class GetAllProjectsQueryHandler : IRequestHandler<GetAllProjectsQuery, Result<IEnumerable<ProjectDto>>>
{
    private readonly IProjectService _projectService;

    public GetAllProjectsQueryHandler(IProjectService projectService)
    {
        _projectService = projectService;
    }

    public async Task<Result<IEnumerable<ProjectDto>>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
    {
        var projects = await _projectService.GetAllAsync(request.UserId);
        return Result.Ok(projects);
    }
}