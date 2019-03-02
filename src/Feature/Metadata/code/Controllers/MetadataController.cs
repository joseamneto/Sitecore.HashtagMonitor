using GoHorse.Feature.Metadata.Repositories;
using Sitecore.Mvc.Controllers;
using Sitecore.Mvc.Presentation;
using System.Web.Mvc;

namespace GoHorse.Feature.Metadata.Controllers
{
    public class MetadataController : SitecoreController
    {
        public MetadataController(MetadataRepository metadataRepository)
        {
            this.MetadataRepository = metadataRepository;
        }

        public ActionResult PageMetadata()
        {
            var metadata = this.MetadataRepository.Get(RenderingContext.Current.Rendering.Item);
            return this.View(metadata);
        }

        public MetadataRepository MetadataRepository { get; private set; }
    }
}