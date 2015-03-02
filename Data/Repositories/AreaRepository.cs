using Data.Infrastructure;
using Domain;
using Data.IRepositories;

namespace Data.Repositories
{
    public class AreaRepository : EntityRepository<Area>, IAreaRepository
    {
        public AreaRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
