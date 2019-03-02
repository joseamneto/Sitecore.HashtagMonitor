using GoHorse.Foundation.DependencyInjection;

namespace GoHorse.Feature.Metadata.Infrastructure.Pipelines.GetPageMetadata
{
    [Service]
    public class CombineTitle
    {
        public void Process(GetPageMetadataArgs args)
        {
            args.Metadata.Title = args.Metadata.PageTitle + args.Metadata.SiteTitle;
        }
    }
}