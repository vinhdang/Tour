using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data;

namespace Data.Infrastructure
{
    public class DatabaseFactory : Disposable, IDatabaseFactory
    {
        private WebContext dataContext;
        public WebContext Get()
        {
            return dataContext ?? (dataContext = new WebContext());
        }
        protected override void DisposeCore()
        {
            if (dataContext != null)
                dataContext.Dispose();
        }
    }
}
