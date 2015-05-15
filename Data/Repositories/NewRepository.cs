
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Infrastructure;
using Domain;
using Data.IRepositories;

namespace Data.Repositories
{
    public class NewRepository : EntityRepository<New>, INewRepository
    {
        public NewRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
