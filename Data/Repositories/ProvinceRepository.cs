
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Infrastructure;
using Domain;
using Data.IRepositories;

namespace Data.Repositories
{
    public class ProvinceRepository : EntityRepository<Province>, IProvinceRepository
    {
        public ProvinceRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
