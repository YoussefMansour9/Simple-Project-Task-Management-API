using FluentResults;
using MediatR;
using TaskManagement.Application.DTOs.Project;

namespace TaskManagement.Application.Features.Project.Commands;

public record CreateProjectCommand(CreateProjectDto Dto, Guid UserId) : IRequest<Result<ProjectDto>>;