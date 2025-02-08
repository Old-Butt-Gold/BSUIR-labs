using DistComp.Data;
using DistComp.Models;
using DistComp.Repositories.Interfaces;

namespace DistComp.Repositories.Implementations;

public class DatabaseUserRepository : BaseDatabaseRepository<User>, IUserRepository
{
    public DatabaseUserRepository(AppDbContext context) : base(context)
    {
        
    }
}