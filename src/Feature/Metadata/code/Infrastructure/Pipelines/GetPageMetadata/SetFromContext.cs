using GoHorse.Foundation.DependencyInjection;
using GoHorse.Foundation.SitecoreExtensions.Extensions;
using Sitecore;
using Sitecore.Data.Items;

namespace GoHorse.Feature.Metadata.Infrastructure.Pipelines.GetPageMetadata
{
    [Service]
    public class SetFromContext
    {
        public void Process(GetPageMetadataArgs args)
        {
            args.Metadata.SiteTitle = this.GetSiteTitle(args.Item);
        }

        private string GetSiteTitle(Item item)
        {
            var contextItem = this.GetSiteMetadataItem(item);
            return contextItem?[Templates.SiteMetadata.Fields.SiteBrowserTitle] ?? string.Empty;
        }

        private Item GetSiteMetadataItem(Item contextItem)
        {
            return contextItem.GetAncestorOrSelfOfTemplate(Templates.SiteMetadata.ID) ?? Context.Site.GetContextItem(Templates.SiteMetadata.ID);
        }
    }
}