using Data.IRepositories;
using Data.Infrastructure;
using Domain;
using Service.Base;
using Service.IServices;

namespace Service.Implementations
{

    public class TourPictureService : EntityService<ITourPictureRepository, TourPicture>, ITourPictureService
    {
        public TourPictureService(ITourPictureRepository paramRepository, IUnitOfWork unitOfWork)
            : base(paramRepository, unitOfWork)
        {

        }
    }
}
