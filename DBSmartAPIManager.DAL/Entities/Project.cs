using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBSmartAPIManager.DAL.Entities
{
    public class Project
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }
        public DateTime UploadDate { get; set; } = DateTime.Now;

        public int UserId { get; set; }

    }
}
