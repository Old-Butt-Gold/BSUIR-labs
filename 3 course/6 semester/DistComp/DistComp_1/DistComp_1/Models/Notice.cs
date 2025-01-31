namespace DistComp_1.Models;

public class Notice : BaseModel
{
    public long StoryId { get; set; }
    public Story Story { get; set; }
    
    public string Content { get; set; }
}