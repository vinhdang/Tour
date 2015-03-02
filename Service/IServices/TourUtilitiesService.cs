using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.IRepositories;
using Data.Infrastructure;
using Domain;
using Service.Base;

namespace Service.IServices
{
    public class TourUtilitiesService : EntityService<ITourServiceRepository, TourSuggestion>, ITourUtilitiesService
    {
        public TourUtilitiesService(ITourServiceRepository paramRepository, IUnitOfWork unitOfWork)
            : base(paramRepository, unitOfWork)
        {

        }
    }
}
