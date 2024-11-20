namespace UserMicroservice.Models.DTO;

/// <summary>
/// User data transfer object containing email and plain text password.
/// </summary>
public class UserRegistrationDto
{
    /// <summary>
    /// Gets or sets the email of the user.
    /// </summary>
    public string Email { get; set; }
    
    /// <summary>
    /// Gets or sets the username of the user
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Gets or sets the password of the user.
    /// </summary>
    public string Password { get; set; }
}