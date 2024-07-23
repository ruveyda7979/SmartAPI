using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBSmartAPIManager.DAL.Entities
{
    public class ProjectFile
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string FileName { get; set; }
        public string FileWay { get; set; }
        public byte[] FileContent { get; set; }
        public DateTime UploadDate { get; set; }
    }
}
