using System.Text.Json.Serialization;

namespace UserMicroservice.Models;

/// <summary>
/// Represents the profile settings for a user.
/// </summary>
public class ProfileSetting
{
    /// <summary>
    /// Gets or sets the unique identifier for the profile settings.
    /// </summary>
    public int SettingsId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the associated user.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the theme preference for the user's profile.
    /// </summary>
    public string? Theme { get; set; }

    /// <summary>
    /// Gets or sets the language preference for the user's profile.
    /// </summary>
    public string? Language { get; set; }

    /// <summary>
    /// Navigation property to the associated user.
    /// </summary>
    [JsonIgnore]
    public virtual User User { get; set; } = null!;
}