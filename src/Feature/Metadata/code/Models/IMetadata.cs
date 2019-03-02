using System.Collections.Generic;

namespace GoHorse.Feature.Metadata.Models
{
    public interface IMetadata
    {
        string PageTitle { get; set; }
        string SiteTitle { get; set; }
        string Description { get; set; }
        ICollection<string> KeywordsList { get; }
        string Title { get; set; }
        ICollection<string> Robots { get; }
        ICollection<KeyValuePair<string, string>> CustomMetadata { get; }
    }
}