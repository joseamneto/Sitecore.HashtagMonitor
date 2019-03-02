using GoHorse.Feature.Metadata.Models;
using Sitecore.Data.Items;

namespace GoHorse.Feature.Metadata.Infrastructure.Pipelines.GetPageMetadata
{
    public class GetPageMetadataArgs : Sitecore.Pipelines.PipelineArgs
    {
        public GetPageMetadataArgs(IMetadata metadata, Item item)
        {
            this.Metadata = metadata;
            this.Item = item;
        }

        public IMetadata Metadata { get; }
        public Item Item { get; }
    }
}