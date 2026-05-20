using AutoMapper;
using FluentResults;
using MediatR;
using TaskManagement.Application.DTOs.Project;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.Features.Project.Commands;

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Result<ProjectDto>>
{
    private readonly IProjectService _projectService;

    public CreateProjectCommandHandler(IProjectService projectService)
    {
        _projectService = projectService;
    }

    public async Task<Result<ProjectDto>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var result = await _projectService.CreateAsync(request.Dto, request.UserId);
        if (result == null)
        {
            return Result.Fail("Failed to create project");
        }
        return Result.Ok(result);
    }
}