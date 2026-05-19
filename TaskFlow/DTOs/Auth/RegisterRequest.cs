using System.ComponentModel.DataAnnotations;

namespace TaskFlow.DTOs.Auth;

public class RegisterRequest
{
    [Required]
    [MinLength(2)]
    [MaxLength(50)]
    public string FirstName { get; set; }
    
    [Required]
    [MinLength(2)]
    [MaxLength(50)]
    public string LastName { get; set; }
    
    [Required]
    [EmailAddress]
    [MaxLength(150)]
    public string Email { get; set; }
    
    [Required]
    [MinLength(8)]
    [MaxLength(100)]
    public string Password { get; set; }
}