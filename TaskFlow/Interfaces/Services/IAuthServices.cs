using TaskFlow.DTOs.Auth;

namespace TaskFlow.Interfaces.Services;

public interface IAuthServices
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
}