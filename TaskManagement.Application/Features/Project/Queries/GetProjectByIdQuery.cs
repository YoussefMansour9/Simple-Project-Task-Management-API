using FluentResults;
using MediatR;
using TaskManagement.Application.DTOs.Project;

namespace TaskManagement.Application.Features.Project.Queries;

public record GetProjectByIdQuery(Guid Id, Guid UserId) : IRequest<Result<ProjectDto>>;