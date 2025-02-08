using DistComp.Data;
using DistComp.Models;
using DistComp.Repositories.Interfaces;

namespace DistComp.Repositories.Implementations;

public class DatabaseStoryRepository : BaseDatabaseRepository<Story>, IStoryRepository
{
    public DatabaseStoryRepository(AppDbContext context) : base(context)
    {
    }
}