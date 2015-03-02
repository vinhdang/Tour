using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Infrastructure;
using Domain;

namespace Data.IRepositories
{

    public interface ICategoryRepository : IRepositoryBase<Category>
    {
        #region Admin
        List<Category> GetAll();
        #endregion
    }
}
