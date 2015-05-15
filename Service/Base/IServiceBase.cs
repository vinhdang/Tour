using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Service.Base
{
    public interface IServiceBase<TEntityType> where TEntityType : class ,new()
    {
        List<TEntityType> GetMany(Func<TEntityType, bool> where);
        IQueryable<TEntityType> AllIncluding(params Expression<Func<TEntityType, object>>[] includeProperties);

        TEntityType Find(long id);
        TEntityType Get(Func<TEntityType, Boolean> where);

        void Insert(TEntityType entity);
        void Update(TEntityType entity);
        void Delete(long id);
        void Delete(TEntityType entity);
        void Save();

        IQueryable<TEntityType> All { get; }
    }
}
