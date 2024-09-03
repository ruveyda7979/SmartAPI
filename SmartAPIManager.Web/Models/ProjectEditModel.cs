using DBSmartAPIManager.DAL.Entities;

namespace SmartAPIManager.Web.Models
{
    public class ProjectEditModel
    {
        
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime UploadDate { get; set; } = DateTime.Now;
        
        public List<IFormFile> ProjectFile { get; set; }
        public List<ProjectFile>? ExistingFiles { get; set; } // Mevcut dosyalar için
    }
}
