using System.ComponentModel.DataAnnotations;

namespace TaskFlow.DTOs.Auth;

public class LoginRequest
{
    [Required]
    [EmailAddress]
    [MaxLength(150)]
    public string Email { get; set; }
    
    [Required]
    [MinLength(8)]
    [MaxLength(100)]
    public string Password { get; set; }
}