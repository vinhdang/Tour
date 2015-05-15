using Data.IRepositories;
using Data.Infrastructure;
using Domain;
using Service.Base;
using Service.IServices;

namespace Service.Implementations
{
  
    public class ConfigService : EntityService<IConfigRepository, Config>, IConfigService
    {
        public ConfigService(IConfigRepository paramRepository, IUnitOfWork unitOfWork)
            : base(paramRepository, unitOfWork)
        {

        }
    }
}
