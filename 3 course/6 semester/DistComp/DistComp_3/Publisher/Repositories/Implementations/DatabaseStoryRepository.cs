using Publisher.Data;
using Publisher.Models;
using Publisher.Repositories.Interfaces;

namespace Publisher.Repositories.Implementations;

public class DatabaseStoryRepository : BaseDatabaseRepository<Story>, IStoryRepository
{
    public DatabaseStoryRepository(AppDbContext context) : base(context)
    {
    }
}