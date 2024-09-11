using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DBSmartAPIManager.DAL.Entities;


namespace DBSmartAPIManager.DAL.Context
{
    public class Repository<T> : IRepository<T> where T : class, new()
    {
        private DBSmartAPIManagerContext _context = null;
        private DbSet<T> dbSet;
        protected Repository(DBSmartAPIManagerContext context)
        {
            _context = context;
            dbSet = _context.Set<T>();
        }

        public async Task<T?> SelectAsync(Expression<Func<T, bool>>? paramerts = null)
        {
            var result = paramerts != null ? await dbSet.FirstOrDefaultAsync(paramerts) : await dbSet.FirstOrDefaultAsync();
            return result;
        }

        public async Task<IEnumerable<T>> SelectManyAsync(Expression<Func<T, bool>>? paramerts = null)
        {
            var result = paramerts != null ? await dbSet.Where(paramerts).ToListAsync() : await dbSet.ToListAsync();
            return result;
        }

        public async Task<IEnumerable<T>> DataSourceAsync(Expression<Func<T, bool>>? paramerts = null)
        {
            var result = paramerts != null ? await dbSet.Where(paramerts).ToListAsync() : await dbSet.ToListAsync();
            return result;
        }

        public async Task<IQueryable<T>> SQLQueryAsync(string sql, params SqlParameter[]? paramerts)
        {
            if (paramerts != null)
            {
                return dbSet.FromSqlRaw(sql, paramerts);
            }
            else
            {
                return dbSet.FromSqlRaw(sql);
            }
        }

        public async Task<IEnumerable<T>> ListClonAsync(IEnumerable<T> oldList)
        {
            var newList = new List<T>();
            foreach (var item in oldList)
            {
                var a = JsonConvert.SerializeObject(item);
                newList.Add(JsonConvert.DeserializeObject<T>(a));
            }
            return await Task.FromResult(newList);
        }

        public async Task<T> ClonAsync(T old)
        {
            var a = JsonConvert.SerializeObject(old);
            var news = JsonConvert.DeserializeObject<T>(a);
            return await Task.FromResult(news);
        }

        public async Task SaveAsync(T val, bool autoSave = true)
        {
            var primaryKey = dbSet.EntityType.FindPrimaryKey()?.Properties.FirstOrDefault();
            var Id = _context.Entry(val).Property(primaryKey.Name).CurrentValue;

            if (Id == null || (int)Id == 0)
            {
                await dbSet.AddAsync(val);
            }
            else
            {
                dbSet.Attach(val);
                _context.Entry(val).State = EntityState.Modified;
            }
            if (autoSave) await _context.SaveChangesAsync();
        }

        public async Task SaveListAsync(List<T> val, bool autoSave = true)
        {
            await dbSet.AddRangeAsync(val);
            if (autoSave) await _context.SaveChangesAsync();
        }

        public async Task UpdateListAsync(List<T> val, bool autoSave = true)
        {
            dbSet.UpdateRange(val);
            if (autoSave) await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T val)
        {
            dbSet.Attach(val);
            _context.Entry(val).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Expression<Func<T, bool>>? paramerts = null, bool autoSave = true)
        {
            //var ent = await SelectAsync(paramerts);
            //if (ent != null)
            //{
            //    dbSet.Remove(ent);
            //    if (autoSave) await _context.SaveChangesAsync();
            //}

            // İlk olarak, silinecek entity'yi ve ilişkili ProjectFile kayıtlarını getiriyoruz
            // İlk olarak, silinecek entity'yi ve ilişkili ProjectFile kayıtlarını getiriyoruz
            // İlk olarak, silinecek entity'yi ve ilişkili ProjectFile kayıtlarını getiriyoruz
            var entity = await dbSet
                .Include(p => (p as Project).ProjectFile) // İlişkili ProjectFile'ları yükle
                .FirstOrDefaultAsync(paramerts); // Burada paramerts kullanılıyor

            if (entity != null)
            {
                // İlişkili ProjectFile kayıtlarını sil
                if (entity is Project project && project.ProjectFile != null && project.ProjectFile.Any())
                {
                    // ProjectFile'ları silmek için doğru dbSet'i kullanıyoruz
                    _context.Set<ProjectFile>().RemoveRange(project.ProjectFile);
                }

                // Ana entity'yi sil
                dbSet.Remove(entity);

                // Değişiklikleri kaydet
                if (autoSave)
                {
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task DeleteAllAsync(Expression<Func<T, bool>>? paramerts = null, bool autoSave = true)
        {
            var ent = await DataSourceAsync(paramerts);
            if (ent != null)
            {
                dbSet.RemoveRange(ent);
                if (autoSave) await _context.SaveChangesAsync();
            }
        }

        public async Task SoftDeleteAsync(Expression<Func<T, bool>>? paramerts = null, Expression<Func<T, object>>? fieldName = null, object? fieldNameValue = null)
        {
            var ent = await dbSet.FirstOrDefaultAsync(paramerts);

            if (ent != null)
            {
                dbSet.Attach(ent);
                _context.Entry(ent).Property(fieldName).CurrentValue = fieldNameValue;
                _context.Entry(ent).Property(fieldName).IsModified = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            if (typeof(T) == typeof(Project))
            {
                return await dbSet
                    .Include("ProjectFile")  // ProjectFile'ı yükler
                    .FirstOrDefaultAsync(entity => EF.Property<int>(entity, "ProjectId") == id) as T;
            }
            else
            {
                return await dbSet.FindAsync(id);
            }
        }

        public Task<IEnumerable<T>> GetByForeignKeyAsync(int foreignKeyId)
        {
            // I don't know this part . What code should I write
            throw new NotImplementedException();
        }


        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }








    }
}
