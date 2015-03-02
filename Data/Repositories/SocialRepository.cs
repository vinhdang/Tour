using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Infrastructure;
using Domain;
using Data.IRepositories;

namespace Data.Repositories
{
    public class SocialRepository : EntityRepository<Social>, ISocialRepository
    {
        public SocialRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
