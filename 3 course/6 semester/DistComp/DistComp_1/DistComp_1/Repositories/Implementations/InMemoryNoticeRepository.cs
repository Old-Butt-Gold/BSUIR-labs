using DistComp_1.Models;
using DistComp_1.Repositories.Interfaces;

namespace DistComp_1.Repositories.Implementations;

public class InMemoryNoticeRepository : BaseInMemoryRepository<Notice>, INoticeRepository
{
    
}