using Microsoft.AspNetCore.Mvc;
using TaskFlow.DTOs.Auth;
using TaskFlow.Interfaces.Services;

namespace TaskFlow.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthServices _authServices;
    
    public AuthController(IAuthServices authServices)
    {
        _authServices = authServices;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
    {
        var response = await _authServices.RegisterAsync(request);
        return Ok(response);
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        var response = await _authServices.LoginAsync(request);
        return Ok(response);
    }
}