using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Publisher.Models;

public class Tag : BaseModel
{
    [Required]
    [MinLength(2)]
    [MaxLength(32)]
    [Column(TypeName = "text")]
    public string Name { get; set; }

    public virtual List<Story> Stories { get; set; } = [];
}