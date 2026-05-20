using FluentResults;
using MediatR;
using TaskManagement.Application.DTOs.Auth;

namespace TaskManagement.Application.Features.Auth.Commands;

public record LoginCommand(LoginDto Dto) : IRequest<Result<AuthResponseDto>>;