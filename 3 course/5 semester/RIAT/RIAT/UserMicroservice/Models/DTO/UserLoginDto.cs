namespace UserMicroservice.Models.DTO;

public class UserLoginDto
{
    /// <summary>
    /// Gets or sets the username or email of the user
    /// </summary>
    public string UsernameOrEmail { get; set; }

    /// <summary>
    /// Gets or sets the password of the user.
    /// </summary>
    public string Password { get; set; }
}