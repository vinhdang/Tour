using Data.Infrastructure;
using Domain;
using Data.IRepositories;

namespace Data.Repositories
{
    public class ProviderPictureRepository : EntityRepository<ProviderPicture>, IProviderPictureRepository
    {
        public ProviderPictureRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
