using DistComp.Data;
using DistComp.Models;
using DistComp.Repositories.Interfaces;

namespace DistComp.Repositories.Implementations;

public class DatabaseTagRepository : BaseDatabaseRepository<Tag>, ITagRepository
{
    public DatabaseTagRepository(AppDbContext context) : base(context)
    {
    }
}