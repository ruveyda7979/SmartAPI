using DBSmartAPIManager.DAL.Context;
using DBSmartAPIManager.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBSmartAPIManager.DAL.Services
{
    public class ProjectFileService:Repository<ProjectFile>
    {
        public ProjectFileService(DBSmartAPIManagerContext context): base(context)
        {    
        }

        
    }
}
