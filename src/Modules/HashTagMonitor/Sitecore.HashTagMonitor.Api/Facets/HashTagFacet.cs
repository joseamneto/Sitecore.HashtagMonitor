using Sitecore.XConnect;

namespace Sitecore.HashTagMonitor.Api.Facets
{
    [FacetKey(FacetName)]
    public class HashTagFacet : Facet
    {
        public const string FacetName = "HashTagFacet";

        public string HashTag { get; set; }
    }
}
