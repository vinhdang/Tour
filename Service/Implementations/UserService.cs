using Data.IRepositories;
using Data.Infrastructure;
using Domain;
using Service.Base;
using Service.IServices;
using System.Linq;

namespace Service.Implementations
{

    public class UserService : EntityService<IUserRepository, User>, IUserService
    {
        public IUserRepository userRepo { get; set; }

        public UserService(IUserRepository paramRepository, IUnitOfWork unitOfWork) : base(paramRepository, unitOfWork)
        {
            this.userRepo = paramRepository;
        }

        public User GetByID(int userid)
        {
            var all = userRepo.All.ToList();
            if (all == null) return null;

            return all.SingleOrDefault(u => u.UserID == userid);
        }
    }
}
