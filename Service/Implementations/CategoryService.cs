using System.Collections.Generic;
using Data.IRepositories;
using Data.Infrastructure;
using Domain;
using Service.Base;
using Service.IServices;
namespace Service.Implementations
{

    public class CategoryService : EntityService<ICategoryRepository, Category>, ICategoryService
    {
        private readonly ICategoryRepository paramRepository;
        private readonly IUnitOfWork unitOfWork;
        public CategoryService(ICategoryRepository paramRepository, IUnitOfWork unitOfWork)
            : base(paramRepository, unitOfWork)
        {
            this.paramRepository = paramRepository;
            this.unitOfWork = unitOfWork;
        }
        #region Admin
        public List<Category> GetAll()
        {
            return paramRepository.GetAll();
        }
       
        #endregion
       
    }
}
