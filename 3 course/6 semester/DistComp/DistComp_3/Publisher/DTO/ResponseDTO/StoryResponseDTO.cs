using Publisher.Models;

namespace Publisher.DTO.ResponseDTO;

public class StoryResponseDTO
{
    public long Id { get; set; }
    public string Title { get; set; }
    
    public long UserId { get; set; }
    public User User { get; set; }

    public string Content { get; set; }
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }

    public List<Tag> Tags { get; set; } = [];
}