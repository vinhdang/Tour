using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.IRepositories;
using Data.Infrastructure;
using Domain;

namespace Data.Repositories
{
    public class TourProviderRepository : EntityRepository<TourProvider>, ITourProviderRepository
    {
        public TourProviderRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
