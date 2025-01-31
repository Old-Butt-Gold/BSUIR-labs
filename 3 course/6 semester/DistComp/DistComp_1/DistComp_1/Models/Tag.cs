namespace DistComp_1.Models;

public class Tag : BaseModel
{
    public string Name { get; set; }

    public List<Story> Stories { get; set; } = [];
}