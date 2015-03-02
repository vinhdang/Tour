using Data.IRepositories;
using Data.Infrastructure;
using Domain;
using Service.Base;
using Service.IServices;

namespace Service.Implementations
{

    public class ProviderPictureService : EntityService<IProviderPictureRepository, ProviderPicture>, IProviderPictureService
    {
        public ProviderPictureService(IProviderPictureRepository paramRepository, IUnitOfWork unitOfWork)
            : base(paramRepository, unitOfWork)
        {

        }
    }
}
