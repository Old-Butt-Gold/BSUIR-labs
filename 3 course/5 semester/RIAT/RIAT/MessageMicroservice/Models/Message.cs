namespace MessageMicroservice.Models;

public class Message
{
    public Guid MessageId { get; set; }

    public Guid? ChatId { get; set; }

    public Guid? SenderId { get; set; }

    public string? Context { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? IsEdited { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? EditedAt { get; set; }
}
