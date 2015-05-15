using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Data;

namespace Data.Infrastructure
{
    public abstract class RepositoryBase
    {
        private WebContext dataContext;
        protected RepositoryBase(IDatabaseFactory IdatabaseFactory)
        {
            DatabaseFactory = IdatabaseFactory;
        }
        protected IDatabaseFactory DatabaseFactory
        {
            get;
            private set;
        }
        protected WebContext DataContext
        {
            get { return dataContext ?? (dataContext = DatabaseFactory.Get()); }
        }
    }
}
