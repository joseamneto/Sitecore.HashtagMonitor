using GoHorse.Feature.Metadata.Infrastructure.Pipelines.GetPageMetadata;
using GoHorse.Feature.Metadata.Models;
using GoHorse.Foundation.DependencyInjection;
using Sitecore.Data.Items;
using Sitecore.Pipelines;
using System;

namespace GoHorse.Feature.Metadata.Repositories
{
    [Service]
    public class MetadataRepository
    {
        public IMetadata Get(Item item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            var args = new GetPageMetadataArgs(new MetadataViewModel(), item);
            CorePipeline.Run("metadata.getPageMetadata", args);

            return args.Metadata;
        }
    }
}