using Data.IRepositories;
using Data.Infrastructure;
using Domain;
using Service.Base;
using Service.IServices;

namespace Service.Implementations
{

    public class TourThemeService : EntityService<ITourThemeRepository, TourTheme>, ITourThemeService
    {
        public TourThemeService(ITourThemeRepository paramRepository, IUnitOfWork unitOfWork)
            : base(paramRepository, unitOfWork)
        {

        }
    }
}
