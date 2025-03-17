using Publisher.Data;
using Publisher.Models;
using Publisher.Repositories.Interfaces;

namespace Publisher.Repositories.Implementations;

public class DatabaseUserRepository : BaseDatabaseRepository<User>, IUserRepository
{
    public DatabaseUserRepository(AppDbContext context) : base(context)
    {
        
    }
}