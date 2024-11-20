namespace MessageMicroservice.Models.DTO;

public class SendMessageRequest
{
    public string Username { get; set; }
    public string ChatName { get; set; }
    public string Content { get; set; }
}
