using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBSmartAPIManager.DAL.Entities
{
    public class ProjectJson
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string JsonName { get; set; }
        public string RequestUrl { get; set; }
        public string Content { get; set; }
        public string RelatedTable { get; set; }
        public string SendPattern { get; set; }
        public string ReceivedPattern { get; set; }
        public DateTime UploadDate { get; set; }

    }
}
