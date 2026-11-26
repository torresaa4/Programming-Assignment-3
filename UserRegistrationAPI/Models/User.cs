using System.ComponentModel.DataAnnotations;

namespace UserRegistrationAPI.Models;

public class User
{
    public int Id { get; set; }
   
    public string Name { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
   
    public string PhoneNumber { get; set; } = string.Empty;
    
    public string Address  { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;
}




