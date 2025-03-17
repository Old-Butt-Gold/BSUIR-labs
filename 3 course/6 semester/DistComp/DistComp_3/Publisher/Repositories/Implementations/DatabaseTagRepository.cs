using Publisher.Data;
using Publisher.Models;
using Publisher.Repositories.Interfaces;

namespace Publisher.Repositories.Implementations;

public class DatabaseTagRepository : BaseDatabaseRepository<Tag>, ITagRepository
{
    public DatabaseTagRepository(AppDbContext context) : base(context)
    {
    }
}