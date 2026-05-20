using FluentResults;
using MediatR;
using TaskManagement.Application.DTOs.Task;

namespace TaskManagement.Application.Features.Task.Commands;

public record CreateTaskCommand(CreateTaskDto Dto, Guid UserId) : IRequest<Result<TaskDto>>;