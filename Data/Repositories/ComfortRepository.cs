
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Infrastructure;
using Domain.Entities;
using Data.IRepositories;

namespace Data.Repositories
{
    public class ComfortRepository : EntityRepository<Comfort>, IComfortRepository
    {
        public ComfortRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
