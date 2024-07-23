using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBSmartAPIManager.DAL.Context
{
    public interface IRepository<T> where T : class,new()
    {
    }
}
