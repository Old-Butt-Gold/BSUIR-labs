using DistComp.Models;

namespace DistComp.DTO.ResponseDTO;

public class NoticeResponseDTO
{
    public long Id { get; set; }
    
    public long StoryId { get; set; }
    public Story Story { get; set; }
    
    public string Content { get; set; }
}