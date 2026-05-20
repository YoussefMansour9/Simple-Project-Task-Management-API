using FluentResults;
using MediatR;
using TaskManagement.Application.DTOs.Task;

namespace TaskManagement.Application.Features.Task.Commands;

public record UpdateTaskCommand(UpdateTaskDto Dto, Guid UserId) : IRequest<Result<TaskDto>>;