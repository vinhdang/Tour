using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Data.Infrastructure
{
    public interface IRepositoryBase<TEntityType> where TEntityType : class
    {
        List<TEntityType> GetMany(Func<TEntityType, bool> where);
        IQueryable<TEntityType> AllIncluding(params Expression<Func<TEntityType, object>>[] includeProperties);

        TEntityType Find(long id);
        TEntityType Get(Func<TEntityType, Boolean> where);

        void Insert(TEntityType entity);
        void Update(TEntityType products);
        void Delete(long id);
        void Delete(TEntityType entity);

        IQueryable<TEntityType> All { get; }
    }
}
