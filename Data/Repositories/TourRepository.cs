using Data.Infrastructure;
using Domain;
using Data.IRepositories;

namespace Data.Repositories
{
    public class TourRepository : EntityRepository<Tour>, ITourRepository
    {
        public TourRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
