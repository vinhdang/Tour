using Data.Infrastructure;
using Domain;
using Data.IRepositories;

namespace Data.Repositories
{
    public class TourPriceRepository : EntityRepository<TourPrice>, ITourPriceRepository
    {
        public TourPriceRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
