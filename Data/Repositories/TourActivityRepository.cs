using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.IRepositories;
using Data.Infrastructure;
using Domain;

namespace Data.Repositories
{
    public class TourActivityRepository : EntityRepository<TourActivity>, ITourActivityRepository
    {
        public TourActivityRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
