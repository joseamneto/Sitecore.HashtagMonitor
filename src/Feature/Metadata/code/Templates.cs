using Sitecore.Data;

namespace GoHorse.Feature.Metadata
{
    public struct Templates
    {
        public struct PageMetadata
        {
            public static ID ID = new ID("{FFF03E52-A60E-474B-8A73-387AC7906079}");

            public struct Fields
            {
                public static readonly ID BrowserTitle = new ID("{EBB4B619-DF85-4D8C-B03D-4CBD0B580C4E}");
                public static readonly ID Description = new ID("{5D6B2190-3457-47E3-81C7-E98FADEBA8AB}");
                public static readonly ID Keywords = new ID("{2E1D4B22-B295-4A0B-AD21-A42470F3731D}");
                public static readonly ID CanIndex = new ID("{D16BD920-2A52-4A6B-AF12-3605FB42FECD}");
                public static readonly ID CanFollow = new ID("{9B0E2522-CF10-4236-8559-4F2CDD29F4C3}");
                public static readonly ID CustomMetadata = new ID("{956ED28C-1F32-4E97-B6A2-88DDAF4447CD}");
            }
        }

        public struct SiteMetadata
        {
            public static readonly ID ID = new ID("{D4C4397A-8BCE-4B96-BBDA-3EDB9657409E}");

            public struct Fields
            {
                public static readonly ID SiteBrowserTitle = new ID("{13E6B703-8733-4552-B412-57D0AFA3E8EC}");
            }
        }

        public struct Keyword
        {
            public static ID ID = new ID("{7E8E62D9-E172-400B-A0EE-458D2B7B7CFD}");

            public struct Fields
            {
                public static readonly ID Keyword = new ID("{85AAE43E-8A6A-4D13-8940-6C8662CDE017}");
            }
        }
    }
}