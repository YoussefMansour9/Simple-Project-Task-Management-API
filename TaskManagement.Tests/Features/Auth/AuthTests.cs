using Moq;
using FluentResults;
using Xunit;
using FluentAssertions;
using TaskManagement.Application.DTOs.Auth;
using TaskManagement.Application.Features.Auth.Commands;
using TaskManagement.Application.Features.Auth.Queries;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Tests.Features.Auth;

public class RegisterCommandHandlerTests
{
    private readonly Mock<IAuthService> _mockAuthService;
    private readonly RegisterCommandHandler _handler;

    public RegisterCommandHandlerTests()
    {
        _mockAuthService = new Mock<IAuthService>();
        _handler = new RegisterCommandHandler(_mockAuthService.Object);
    }

    [Fact]
    public async System.Threading.Tasks.Task Handle_ValidRequest_ReturnsSuccess()
    {
        var dto = new RegisterDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            Password = "Password123"
        };

        var expectedResponse = new AuthResponseDto
        {
            Token = "valid-token",
            Email = dto.Email,
            FullName = $"{dto.FirstName} {dto.LastName}",
            ExpiresAt = DateTime.UtcNow.AddHours(1)
        };

        _mockAuthService.Setup(x => x.RegisterAsync(dto))
            .ReturnsAsync(expectedResponse);

        var command = new RegisterCommand(dto);
        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Token.Should().Be("valid-token");
    }

    [Fact]
    public async System.Threading.Tasks.Task Handle_InvalidEmail_ReturnsFailure()
    {
        var dto = new RegisterDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "invalid-email",
            Password = "Password123"
        };

        _mockAuthService.Setup(x => x.RegisterAsync(dto))
            .ThrowsAsync(new InvalidOperationException("Invalid email"));

        var command = new RegisterCommand(dto);
        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Message == "Invalid email");
    }
}

public class LoginCommandHandlerTests
{
    private readonly Mock<IAuthService> _mockAuthService;
    private readonly LoginCommandHandler _handler;

    public LoginCommandHandlerTests()
    {
        _mockAuthService = new Mock<IAuthService>();
        _handler = new LoginCommandHandler(_mockAuthService.Object);
    }

    [Fact]
    public async System.Threading.Tasks.Task Handle_ValidCredentials_ReturnsSuccess()
    {
        var dto = new LoginDto
        {
            Email = "john@example.com",
            Password = "Password123"
        };

        var expectedResponse = new AuthResponseDto
        {
            Token = "valid-token",
            Email = dto.Email,
            FullName = "John Doe",
            ExpiresAt = DateTime.UtcNow.AddHours(1)
        };

        _mockAuthService.Setup(x => x.LoginAsync(dto))
            .ReturnsAsync(expectedResponse);

        var command = new LoginCommand(dto);
        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
    }

    [Fact]
    public async System.Threading.Tasks.Task Handle_InvalidCredentials_ReturnsFailure()
    {
        var dto = new LoginDto
        {
            Email = "john@example.com",
            Password = "WrongPassword"
        };

        _mockAuthService.Setup(x => x.LoginAsync(dto))
            .ThrowsAsync(new UnauthorizedAccessException("Invalid credentials"));

        var command = new LoginCommand(dto);
        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
    }
}

public class GetCurrentUserQueryHandlerTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly GetCurrentUserQueryHandler _handler;

    public GetCurrentUserQueryHandlerTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _handler = new GetCurrentUserQueryHandler(_mockUserRepository.Object);
    }

    [Fact]
    public async System.Threading.Tasks.Task Handle_ExistingUser_ReturnsSuccess()
    {
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com"
        };

        _mockUserRepository.Setup(x => x.GetByIdAsync(userId))
            .ReturnsAsync(user);

        var query = new GetCurrentUserQuery(userId);
        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Email.Should().Be("john@example.com");
    }

    [Fact]
    public async System.Threading.Tasks.Task Handle_NonExistingUser_ReturnsFailure()
    {
        var userId = Guid.NewGuid();
        _mockUserRepository.Setup(x => x.GetByIdAsync(userId))
            .ReturnsAsync((User?)null);

        var query = new GetCurrentUserQuery(userId);
        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Message == "User not found");
    }
}