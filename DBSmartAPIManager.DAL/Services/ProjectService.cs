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
    public class ProjectService:Repository<Project>
    {
        private readonly DBSmartAPIManagerContext _context;
        public ProjectService(DBSmartAPIManagerContext context) : base(context) 
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Attach<TEntity>(TEntity entity) where TEntity : class
        {
            if (_context == null)
                throw new InvalidOperationException("Context is not initialized.");

            _context.Set<TEntity>().Attach(entity);
        }
    }
}
