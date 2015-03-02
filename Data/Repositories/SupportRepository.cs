using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Infrastructure;
using Data.IRepositories;
using Domain;

namespace Data.Repositories
{
    public class SupportRepository : EntityRepository<Support>, ISupportRepository
    {
        public SupportRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }
    }

}
