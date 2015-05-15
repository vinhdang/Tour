using Data.IRepositories;
using Data.Infrastructure;
using Domain;
using Service.Base;
using Service.IServices;

namespace Service.Implementations
{

    public class TourPriceService : EntityService<ITourPriceRepository, TourPrice>, ITourPriceService
    {
        public TourPriceService(ITourPriceRepository paramRepository, IUnitOfWork unitOfWork)
            : base(paramRepository, unitOfWork)
        {

        }
    }
}
