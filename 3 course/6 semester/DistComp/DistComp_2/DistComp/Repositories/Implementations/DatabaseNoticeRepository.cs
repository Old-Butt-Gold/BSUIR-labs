using DistComp.Data;
using DistComp.Models;
using DistComp.Repositories.Interfaces;

namespace DistComp.Repositories.Implementations;

public class DatabaseNoticeRepository : BaseDatabaseRepository<Notice>, INoticeRepository
{
    public DatabaseNoticeRepository(AppDbContext context) : base(context)
    {
    }
}