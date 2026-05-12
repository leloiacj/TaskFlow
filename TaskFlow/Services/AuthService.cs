using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Data;
using TaskFlow.DTOs.Auth;
using TaskFlow.Interfaces.Services;
using TaskFlow.Models;

namespace TaskFlow.Services;

public class AuthService : IAuthServices
{
    private readonly AppDbContext _context;
    private readonly ITokenService _tokenService;

    public AuthService(AppDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var normalizedEmail = request.Email.Trim().ToLower();

        var emailalreadyExist = await _context.Users.AnyAsync(user => user.Email == normalizedEmail);

        if (emailalreadyExist)
        {
            throw new InvalidOperationException("Email is already in use");
        }

        var user = new AppUser
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Email = normalizedEmail,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            CreatedAt = DateTime.UtcNow,
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var token = _tokenService.GenerateToken(user);

        return new AuthResponse
        {
            UserId = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Token = token,
        };
    }
    

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var normalizedEmail = request.Email.Trim().ToLower();
            
            var user = await _context.Users.FirstOrDefaultAsync(user => user.Email == normalizedEmail);

            if (user is null)
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }
            
            var passwordIsValid = BCrypt.Net.BCrypt.Verify(
                request.Password,
                user.PasswordHash);

            if (!passwordIsValid)
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }
            
            var token = _tokenService.GenerateToken(user);

            return new AuthResponse
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Token = token,
            };
        }
}
