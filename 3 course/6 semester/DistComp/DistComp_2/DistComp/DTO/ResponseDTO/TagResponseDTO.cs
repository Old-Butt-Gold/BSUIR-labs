using DistComp.Models;

namespace DistComp.DTO.ResponseDTO;

public class TagResponseDTO
{
    public long Id { get; set; }
    public string Name { get; set; }
    
    public List<Story> Stories { get; set; } = [];
}