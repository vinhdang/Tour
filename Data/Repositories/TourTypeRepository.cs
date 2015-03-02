using Data.Infrastructure;
using Domain;
using Data.IRepositories;

namespace Data.Repositories
{
    public class TourTypeRepository : EntityRepository<TourType>, ITourTypeRepository
    {
        public TourTypeRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
