namespace ChatMicroservice.Models;

public class Chat
{
    public Guid ChatId { get; set; }

    public string? ChatName { get; set; }

    public bool? IsGroup { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<ChatMember> ChatMembers { get; set; } = new List<ChatMember>();
}
