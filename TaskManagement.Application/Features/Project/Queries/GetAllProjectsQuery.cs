using FluentResults;
using MediatR;
using TaskManagement.Application.DTOs.Project;

namespace TaskManagement.Application.Features.Project.Queries;

public record GetAllProjectsQuery(Guid UserId) : IRequest<Result<IEnumerable<ProjectDto>>>;