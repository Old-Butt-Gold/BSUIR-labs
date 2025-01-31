namespace DistComp_1.Models;

public class User : BaseModel
{
    public string Login { get; set; }
    public string Password { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }

    public List<Story> Stories { get; set; } = [];
}