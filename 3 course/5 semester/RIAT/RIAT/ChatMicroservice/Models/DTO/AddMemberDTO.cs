namespace ChatMicroservice.Models.DTO;

public class AddMemberDto
{
    public string Username { get; set; }
    public Guid ChatId { get; set; }
}