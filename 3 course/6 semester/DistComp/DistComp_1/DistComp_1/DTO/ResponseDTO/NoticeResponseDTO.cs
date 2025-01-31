using DistComp_1.Models;

namespace DistComp_1.DTO.ResponseDTO;

public class NoticeResponseDTO
{
    public long Id { get; set; }
    
    public long StoryId { get; set; }
    public Story Story { get; set; }
    
    public string Content { get; set; }
}