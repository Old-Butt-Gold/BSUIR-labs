using Cassandra.Mapping.Attributes;

namespace Discussion.Models;

[Table("tbl_notice")]
public class Notice : BaseModel
{
    [ClusteringKey]
    public long StoryId { get; set; }
    
    public string Content { get; set; }
}