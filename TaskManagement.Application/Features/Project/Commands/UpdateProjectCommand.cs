using FluentResults;
using MediatR;
using TaskManagement.Application.DTOs.Project;

namespace TaskManagement.Application.Features.Project.Commands;

public record UpdateProjectCommand(UpdateProjectDto Dto, Guid UserId) : IRequest<Result<ProjectDto>>;