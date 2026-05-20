using FluentResults;
using MediatR;

namespace TaskManagement.Application.Features.Task.Commands;

public record DeleteTaskCommand(Guid Id, Guid UserId) : IRequest<Result<bool>>;