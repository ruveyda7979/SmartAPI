using DBSmartAPIManager.DAL.Context;
using DBSmartAPIManager.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBSmartAPIManager.DAL.Services
{
    public class UserService:Repository<User>
    {
        public UserService(DBSmartAPIManagerContext context) : base(context) 
        {
            
        }
    }
}
