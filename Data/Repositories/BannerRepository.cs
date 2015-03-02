using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Infrastructure;
using Domain.Entities;
using Data.IRepositories;

namespace Data.Repositories
{
    public class BannerRepository : EntityRepository<Banner>, IBannerRepository
    {
        public BannerRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
