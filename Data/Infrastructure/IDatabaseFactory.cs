using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data;


namespace Data.Infrastructure
{
    public interface IDatabaseFactory
    {
        WebContext Get();
    }
}
