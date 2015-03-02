using Data.IRepositories;
using Data.Infrastructure;
using Domain;
using Service.Base;
using Service.IServices;

namespace Service.Implementations
{
  
    public class TourTypeService : EntityService<ITourTypeRepository, TourType>, ITourTypeService
    {
        public TourTypeService(ITourTypeRepository paramRepository, IUnitOfWork unitOfWork)
            : base(paramRepository, unitOfWork)
        {

        }
    }
}
