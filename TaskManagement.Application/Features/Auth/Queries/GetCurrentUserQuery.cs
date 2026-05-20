using FluentResults;
using MediatR;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Features.Auth.Queries;

public record GetCurrentUserQuery(Guid UserId) : IRequest<Result<User>>;