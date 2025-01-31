using DistComp_1.Models;

namespace DistComp_1.DTO.ResponseDTO;

public class TagResponseDTO
{
    public long Id { get; set; }
    public string Name { get; set; }
    
    public List<Story> Stories { get; set; } = [];
}