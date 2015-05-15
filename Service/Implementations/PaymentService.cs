using Data.IRepositories;
using Data.Infrastructure;
using Domain;
using Service.Base;
using Service.IServices;

namespace Service.Implementations
{

    public class PaymentService : EntityService<IPaymentRepository, Payment>, IPaymentService
    {
        public PaymentService(IPaymentRepository paramRepository, IUnitOfWork unitOfWork)
            : base(paramRepository, unitOfWork)
        {

        }
    }
}
