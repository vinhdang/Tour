using Data.Infrastructure;
using Domain;
using Data.IRepositories;

namespace Data.Repositories
{
    public class TourPictureRepository : EntityRepository<TourPicture>, ITourPictureRepository
    {
        public TourPictureRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
