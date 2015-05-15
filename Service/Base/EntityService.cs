using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using Data.Infrastructure;

namespace Service.Base
{
    public class EntityService<TIEntityRepository, TEntityType>
        where TIEntityRepository : IRepositoryBase<TEntityType>
        where TEntityType : class
    {

        private readonly TIEntityRepository m_Repository;
        private readonly IUnitOfWork unitOfWork;
        public EntityService(TIEntityRepository paramRepository, IUnitOfWork unitOfWork)
        {
            this.m_Repository = paramRepository;
            this.unitOfWork = unitOfWork;
        }
        public EntityService()
        {

        }
        public IQueryable<TEntityType> All
        {
            get { return m_Repository.All; }
        }
        //$-- Public Methods
        public virtual void Insert(TEntityType entity)
        {
            m_Repository.Insert(entity);
        }

        public virtual void Delete(TEntityType entity)
        {
            m_Repository.Delete(entity);
        }



        public virtual List<TEntityType> GetMany(Func<TEntityType, bool> where)
        {
            return m_Repository.GetMany(where);
        }
        public TEntityType Get(Func<TEntityType, Boolean> where)
        {
            return m_Repository.Get(where);
        }


        public IQueryable<TEntityType> AllIncluding(params Expression<Func<TEntityType, object>>[] includeProperties)
        {
            return m_Repository.AllIncluding(includeProperties);
        }

        public TEntityType Find(long id)
        {
            return m_Repository.Find(id);
        }

        public void Update(TEntityType entity)
        {
            // Existing entity
            m_Repository.Update(entity);
        }

        public void Delete(long id)
        {
            var entity = m_Repository.Find(id);
            m_Repository.Delete(entity);
        }

        public void Save()
        {
            unitOfWork.Commit();
        }
    }
}
