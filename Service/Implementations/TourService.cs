using Data.IRepositories;
using Data.Infrastructure;
using Domain;
using Service.Base;
using Service.IServices;

namespace Service.Implementations
{

    public class TourService : EntityService<ITourRepository, Tour>, ITourService
    {
        public TourService(ITourRepository paramRepository, IUnitOfWork unitOfWork): base(paramRepository, unitOfWork)
        {

        }
    }
}
