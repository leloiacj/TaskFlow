using TaskFlow.Models;

namespace TaskFlow.Interfaces.Services;

public interface ITokenService
{
    string GenerateToken(AppUser user);
}