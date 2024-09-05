using DBSmartAPIManager.DAL.Entities;

namespace SmartAPIManager.Web.Models
{
    public class ProjectJsonViewModel
    {
        public int ProjectJsonId { get; set; }
        public int ProjectId { get; set; }
        public string JsonName { get; set; }
        public DateTime Date { get; set; }
        public string RequestURL { get; set; }
        public string RelatedTable { get; set; }
        public string Content { get; set; }
        public string SendPattern { get; set; }
        public string ReceivedPattern { get; set; }

        public List<ProjectJson>JsonList { get; set; } = new List<ProjectJson>(); // Listeyi başlattık
    }
}
