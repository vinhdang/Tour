using Domain;
using Service.Base;

namespace Service.IServices
{
    public interface IUserService : IServiceBase<User>
    {
        User GetByID(int userid);
    }
}
