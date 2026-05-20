using FluentResults;
using MediatR;
using TaskManagement.Application.DTOs.Task;

namespace TaskManagement.Application.Features.Task.Queries;

public record GetTasksByProjectQuery(Guid ProjectId, Guid UserId) : IRequest<Result<IEnumerable<TaskDto>>>;