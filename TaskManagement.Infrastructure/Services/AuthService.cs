using AutoMapper;
using FluentResults;
using TaskManagement.Application.DTOs.Auth;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;
    private readonly IMapper _mapper;

    public AuthService(IUserRepository userRepository, IJwtService jwtService, IMapper mapper)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
        _mapper = mapper;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
    {
        var existingUser = await _userRepository.GetByEmailAsync(registerDto.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("User with this email already exists");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Email = registerDto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
            Role = "User",
            CreatedAt = DateTime.UtcNow
        };

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        var token = _jwtService.GenerateToken(user.Id, user.Email, user.Role);
        var expiresAt = _jwtService.GetTokenExpiration();

        return new AuthResponseDto
        {
            Token = token,
            Email = user.Email,
            FullName = $"{user.FirstName} {user.LastName}",
            ExpiresAt = expiresAt
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
    {
        var user = await _userRepository.GetByEmailAsync(loginDto.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }

        var token = _jwtService.GenerateToken(user.Id, user.Email, user.Role);
        var expiresAt = _jwtService.GetTokenExpiration();

        return new AuthResponseDto
        {
            Token = token,
            Email = user.Email,
            FullName = $"{user.FirstName} {user.LastName}",
            ExpiresAt = expiresAt
        };
    }
}