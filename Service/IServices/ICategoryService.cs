using System.Collections.Generic;
using Domain;
using Service.Base;

namespace Service.IServices
{
    public interface ICategoryService : IServiceBase<Category>
    {
        #region Admin
        List<Category> GetAll();
        #endregion
    }
}
