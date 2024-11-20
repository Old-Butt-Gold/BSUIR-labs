using System.ComponentModel.DataAnnotations.Schema;

namespace ChatMicroservice.Models;

public class ChatMember
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ChatMemberId { get; set; }

    public Guid ChatId { get; set; }

    public Guid? UserId { get; set; }

    public DateTime? JoinedAt { get; set; }

    public bool? NotificationsEnabled { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]
    public virtual Chat Chat { get; set; } = null!;
}
