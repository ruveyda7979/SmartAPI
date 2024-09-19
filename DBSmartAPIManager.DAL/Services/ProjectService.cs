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

        public async Task<Project?> GetByIdWithFilesAsync(int id)
        {
            return await _context.Project
                .Include(p => p.ProjectFile) // Sadece ProjectFile ilişkisini yüklüyoruz
                .FirstOrDefaultAsync(p => p.ProjectId == id);
        }

        public async Task DeleteProjectWithFilesAsync(int projectId)
        {
            // Projeyi ve ilişkili ProjectFile'ları Include ederek alıyoruz
            var project = await _context.Project
                .Include(p => p.ProjectFile) // Proje dosyalarını yüklüyoruz
                .FirstOrDefaultAsync(p => p.ProjectId == projectId);

            if (project != null)
            {
                // İlişkili ProjectFile'ları önce siliyoruz
                if (project.ProjectFile.Any())
                {
                    _context.ProjectFile.RemoveRange(project.ProjectFile);
                }

                // Sonrasında projeyi siliyoruz
                _context.Project.Remove(project);

                // Değişiklikleri veritabanına kaydediyoruz
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveFileAsync(ProjectFile fileToDelete)
        {
            // Dosyayı veritabanından kaldırıyoruz
            _context.ProjectFile.Remove(fileToDelete);
            await _context.SaveChangesAsync();
        }
    }
}
