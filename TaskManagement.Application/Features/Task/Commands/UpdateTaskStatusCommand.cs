using FluentResults;
using MediatR;
using TaskManagement.Application.DTOs.Task;

namespace TaskManagement.Application.Features.Task.Commands;

public record UpdateTaskStatusCommand(Guid Id, string Status, Guid UserId) : IRequest<Result<TaskDto>>;