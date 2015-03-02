
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Infrastructure;
using Domain;
using Data.IRepositories;

namespace Data.Repositories
{
       public class ContactRepository : EntityRepository<Contact>, IContactRepository
    {
           public ContactRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
