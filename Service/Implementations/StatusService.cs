using Data.IRepositories;
using Data.Infrastructure;
using Domain;
using Service.Base;
using Service.IServices;

namespace Service.Implementations
{

    public class StatusService : EntityService<IStatusRepository, Status>, IStatusService
    {
        public StatusService(IStatusRepository paramRepository, IUnitOfWork unitOfWork)
            : base(paramRepository, unitOfWork)
        {

        }
    }
}
