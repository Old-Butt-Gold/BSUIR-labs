using System.ComponentModel.DataAnnotations;

namespace UserMicroservice.Models.DTO
{
    /// <summary>
    /// Data Transfer Object for updating user profile settings.
    /// </summary>
    public class UserProfileUpdateDto
    {
        /// <summary>
        /// Gets or sets the username for the user.
        /// </summary>
        [StringLength(50, ErrorMessage = "Username must be at most 50 characters long.")]
        public string? Username { get; set; }

        /// <summary>
        /// Gets or sets the profile picture URL for the user.
        /// </summary>
        public string? ProfilePicture { get; set; }

        /// <summary>
        /// Gets or sets the theme preference for the user's profile.
        /// </summary>
        public string? Theme { get; set; }

        /// <summary>
        /// Gets or sets the language preference for the user's profile.
        /// </summary>
        public string? Language { get; set; }
    }
}