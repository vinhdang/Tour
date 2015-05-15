using Data.IRepositories;
using Data.Infrastructure;
using Domain;
using Service.Base;
using Service.IServices;

namespace Service.Implementations
{
    public class BannerService : EntityService<IBannerRepository, Banner>, IBannerService
    {
        public BannerService(IBannerRepository paramRepository, IUnitOfWork unitOfWork)
            : base(paramRepository, unitOfWork)
        {

        }
    }
}
