using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.IRepositories;
using Data.Infrastructure;
using Domain;
using Service.Base;
using Service.IServices;

namespace Service.Implementations
{
    public class TourActivityService : EntityService<ITourActivityRepository, TourActivity>, ITourActivityService
    {
        public TourActivityService(ITourActivityRepository paramRepository, IUnitOfWork unitOfWork)
            : base(paramRepository, unitOfWork)
        {

        }
    }
}
