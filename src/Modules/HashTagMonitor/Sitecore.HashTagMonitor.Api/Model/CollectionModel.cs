using Sitecore.HashTagMonitor.Api.Facets;
using Sitecore.XConnect;
using Sitecore.XConnect.Schema;

namespace Sitecore.HashTagMonitor.Api.Model
{
    public class CollectionModel
    {
        public static XdbModel Model { get; } = BuildModel();

        private static XdbModel BuildModel()
        {
            var builder = new XdbModelBuilder("CollectionModel", new XdbModelVersion(1, 0));
            builder.ReferenceModel(Sitecore.XConnect.Collection.Model.CollectionModel.Model);
            builder.DefineFacet<Interaction, HashTagFacet>(HashTagFacet.FacetName);
            return builder.BuildModel();
        }
    }
}
