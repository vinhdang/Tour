using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Infrastructure;
using Data.IRepositories;
using Domain;

namespace Data.Repositories
{
    public class RolePermissionRepository : EntityRepository<RolePermission>, IRolePermissionRepository
    {
        public RolePermissionRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
