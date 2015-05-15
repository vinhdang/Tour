using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Data;

namespace Data.Infrastructure
{
    public class EntityRepository<TEntityType> : RepositoryBase where TEntityType : class
    {
        //$--Privates
        private readonly IDbSet<TEntityType> dbset;
        //$--Properties

        public IQueryable<TEntityType> All
        {
            get { return dbset.AsQueryable(); }
        }
        //$-- Ctor
        public EntityRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
            dbset = DataContext.Set<TEntityType>();
        }
        //$-- Public Methods
        public virtual void Insert(TEntityType entity)
        {
            dbset.Add(entity);
        }

        public virtual void Delete(TEntityType entity)
        {
            dbset.Remove(entity);
        }



        public virtual List<TEntityType> GetMany(Func<TEntityType, bool> where)
        {
            return dbset.Where(where).ToList();
        }
        public TEntityType Get(Func<TEntityType, Boolean> where)
        {
            return dbset.Where(where).FirstOrDefault<TEntityType>();
        }


        public IQueryable<TEntityType> AllIncluding(params Expression<Func<TEntityType, object>>[] includeProperties)
        {
            IQueryable<TEntityType> query = dbset;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public TEntityType Find(long id)
        {
            return dbset.Find(id);
        }

        public void Update(TEntityType entity)
        {
            // Existing entity
            DataContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(long id)
        {
            var entity = dbset.Find(id);
            dbset.Remove(entity);
        }
    }
}