using Sitecore.Data;

namespace GoHorse.Feature.Identity
{
    public struct Templates
    {
        public struct Identity
        {
            public static readonly ID ID = new ID("{49AC232C-4436-4EEF-BF98-E941C3DBFE6F}");

            public struct Fields
            {
                public static readonly ID SiteHeadingUpperText = new ID("{60980910-DF3F-4C3D-B534-B1509CDEADFF}");
                public static readonly ID SiteHeadingLowerText = new ID("{8C61EAD7-FB91-4870-9187-E7A42F6C5A0C}");
                public static readonly ID Copyright = new ID("{659B3B8D-5866-4BD3-AAEF-5A542789A470}");
                public static readonly ID OrganisationName = new ID("{CCD0F430-4DB8-4DFD-B57A-19EB2A767995}");
                public static readonly ID OrganisationAddress = new ID("{0EB3097B-0925-49CB-BE65-175189A46DD1}");
                public static readonly ID OrganisationPhone = new ID("{E121E1A4-E270-4051-9D65-52822C0FF1ED}");
                public static readonly ID OrganisationEmail = new ID("{0D080DE0-042F-43AE-90E6-F7A5F63DDAE4}");
            }
        }
    }
}