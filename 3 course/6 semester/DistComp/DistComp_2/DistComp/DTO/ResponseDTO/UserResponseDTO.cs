using DistComp.Models;

namespace DistComp.DTO.ResponseDTO;

public class UserResponseDTO
{
    public long Id { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }

    public List<Story> Stories { get; set; } = [];
}