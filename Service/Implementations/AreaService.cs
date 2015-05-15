using Data.IRepositories;
using Data.Infrastructure;
using Domain;
using Service.Base;
using Service.IServices;

namespace Service.Implementations
{

    public class AreaService : EntityService<IAreaRepository, Area>, IAreaService
    {
        public AreaService(IAreaRepository paramRepository, IUnitOfWork unitOfWork)
            : base(paramRepository, unitOfWork)
        {

        }
    }
}
