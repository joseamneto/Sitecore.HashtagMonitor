using Sitecore.Data;

namespace GoHorse.Foundation.Assets
{
    public struct Templates
    {
        public struct RenderingAssets
        {
            public static readonly ID ID = new ID("{4A3D512A-5A34-4392-911C-0AB028127CF4}");

            public struct Fields
            {
                public static readonly ID ScriptFiles = new ID("{23B9C93B-51CD-4BFA-AE79-B5FE0F0047D3}");
                public static readonly ID StylingFiles = new ID("{69D66587-93C8-4FF9-A44A-B8C68DC0F067}");
                public static readonly ID InlineScript = new ID("{26A5ABB1-2BF3-43F8-812E-9ED37437E5F9}");
                public static readonly ID InlineStyling = new ID("{6092B4A8-41C2-42BC-B59B-B3DADF31473A}");
            }
        }

        public struct PageAssets
        {
            public static readonly ID ID = new ID("{7A08CB04-7F83-4723-8BFB-7F717FE42F41}");

            public struct Fields
            {
                public static readonly ID JavascriptCodeTop = new ID("{0E097F19-6257-47D7-81B3-285A9A26995E}");
                public static readonly ID JavascriptCodeBottom = new ID("{F12984B9-5351-4CED-BAC0-4814CA02F2A9}");
                public static readonly ID CssCode = new ID("{03061E07-A4A4-432A-BD96-69FC9150F8E9}");
                public static readonly ID InheritAssets = new ID("{AF9C4E56-A3F9-4D26-AAEF-FF7CF0E8CFA1}");
            }
        }

        public struct HasTheme
        {
            public static readonly ID ID = new ID("{0BAD9BD5-F065-4D65-A2AD-275C7B3DCA36}");

            public struct Fields
            {
                public static readonly ID Theme = new ID("{D800EA3A-06F2-4C88-9020-1A179B58665B}");
            }
        }
    }
}