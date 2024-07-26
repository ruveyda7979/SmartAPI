using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DBSmartAPIManager.DAL.Context
{
    public interface IRepository<T> where T : class,new()
    {
        Task<T?> SelectAsync(Expression<Func<T, bool>> paramerts);
        Task<IEnumerable<T>> DataSourceAsync(Expression<Func<T, bool>> paramerts);
        Task<IQueryable<T>> SQLQueryAsync(string sql, params SqlParameter[] paramerts);
        Task<IEnumerable<T>> ListClonAsync(IEnumerable<T> oldList);
        Task SaveAsync(T val, bool autoSave = true);
        Task UpdateListAsync(List<T> val, bool autoSave = true);
        Task UpdateAsync(T val);
        Task DeleteAsync(Expression<Func<T, bool>> paramerts, bool autoSave = true);
        Task SaveChangesAsync();

        // Additional Methods
        // avcdefg

        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetByForeignKeyAsync(int foreignKeyId);
    }
}
