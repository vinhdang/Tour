using Data.Infrastructure;
using Domain;
using Data.IRepositories;

namespace Data.Repositories
{
    public class TourThemeRepository : EntityRepository<TourTheme>, ITourThemeRepository
    {
        public TourThemeRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
