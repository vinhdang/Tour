using Data.IRepositories;
using Data.Infrastructure;
using Domain;
using Service.Base;
using Service.IServices;
namespace Service.Implementations
{
    public class RolePermissionService : EntityService<IRolePermissionRepository, RolePermission>, IRolePermissionService
    {
        public RolePermissionService(IRolePermissionRepository paramRepository, IUnitOfWork unitOfWork)
            : base(paramRepository, unitOfWork)
        {

        }
    }
}
