namespace DistComp.DTO.RequestDTO;

public class StoryRequestDTO
{
    public long Id { get; set; }
    public string Title { get; set; }
    
    public long UserId { get; set; }

    public string Content { get; set; }
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }

}