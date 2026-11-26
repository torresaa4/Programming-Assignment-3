using System.ComponentModel.DataAnnotations;


namespace UserRegistrationAPI.Models;


// need another model since the frontend will send that wont all be stored in database
public class RegisterModel
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Phone]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    public string Address { get; set; } = string.Empty;

    [Required]
    [StringLength(15, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;
}



