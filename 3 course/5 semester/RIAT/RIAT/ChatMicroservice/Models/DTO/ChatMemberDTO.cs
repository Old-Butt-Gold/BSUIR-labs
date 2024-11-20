namespace ChatMicroservice.Models.DTO;

public class ChatWithMembersDto
{
    public Guid ChatId { get; set; }
    public string? ChatName { get; set; }
    public bool? IsGroup { get; set; }
    public DateTime? CreatedAt { get; set; }
    public List<ChatMemberDto> Members { get; set; } = [];
}

public class ChatMemberDto
{
    public Guid? UserId { get; set; }
    public DateTime? JoinedAt { get; set; }
    public bool? NotificationsEnabled { get; set; }
}