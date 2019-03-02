using System.Collections.Generic;

namespace GoHorse.Feature.Metadata.Models
{
    public class MetadataViewModel : IMetadata
    {
        public string PageTitle { get; set; }
        public string SiteTitle { get; set; }
        public string Description { get; set; }
        public ICollection<string> KeywordsList { get; } = new List<string>();
        public string Title { get; set; }
        public ICollection<string> Robots { get; } = new List<string>();
        public ICollection<KeyValuePair<string, string>> CustomMetadata { get; } = new List<KeyValuePair<string, string>>();
    }
}