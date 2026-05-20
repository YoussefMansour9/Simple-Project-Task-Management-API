using FluentResults;
using MediatR;
using TaskManagement.Application.DTOs.Task;

namespace TaskManagement.Application.Features.Task.Queries;

public record GetTaskByIdQuery(Guid Id, Guid UserId) : IRequest<Result<TaskDto>>;