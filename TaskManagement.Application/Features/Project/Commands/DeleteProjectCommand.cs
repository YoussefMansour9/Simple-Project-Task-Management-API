using FluentResults;
using MediatR;

namespace TaskManagement.Application.Features.Project.Commands;

public record DeleteProjectCommand(Guid Id, Guid UserId) : IRequest<Result<bool>>;