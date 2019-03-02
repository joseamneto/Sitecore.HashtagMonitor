using Sitecore.Caching;
using Sitecore.Data;

namespace GoHorse.Foundation.Assets.Models
{
    public class AssetRequirementCache : CustomCache
    {
        public AssetRequirementCache(long maxSize) : base("GoHorse.Foundation.AssetRequirements", maxSize)
        {
        }

        public AssetRequirementList Get(ID cacheKey)
        {
            return (AssetRequirementList)this.GetObject(cacheKey.ToString());
        }

        public void Set(ID cacheKey, AssetRequirementList requirementList)
        {
            this.SetObject(cacheKey.ToString(), requirementList);
        }
    }
}