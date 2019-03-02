using Sitecore.Data;

namespace GoHorse.Feature.Teasers
{
    public struct Templates
    {
        public struct TeaserContent
        {
            public static ID ID = new ID("{CE5103F9-606C-472D-8A19-0EC5DC40CFD5}");

            public struct Fields
            {
                public static readonly ID Image = new ID("{D47A8DF6-A0D7-4954-8057-71183A1B113A}");
                public static readonly ID Summary = new ID("{CCB11615-9EA8-49C5-8DA7-9237F41929E6}");
                public static readonly ID Link = new ID("{980DDC72-403B-4646-A099-7B9F56D7312C}");
            }
        }

        public struct TeaserHeadline
        {
            public static ID ID = new ID("{66B75400-5F17-4667-846F-73F7317688AC}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{F1ECD4C7-6F4A-4E29-82A9-A52B13E9EA7B}");
                public static readonly ID Subtitle = new ID("{4F9A26AC-2B74-4ED9-AC3F-E1D6B6BE2E5D}");
            }
        }

        public struct CallToActionButton
        {
            public static ID ID = new ID("{D38A54BB-0133-4524-A80F-F3ED6EAB6187}");

            public struct Fields
            {
                public static readonly ID Link = new ID("{1DFF7A0C-1DC4-41CF-91CC-5082FB8FB80A}");
                public static readonly ID Label = new ID("{0E21EE0F-4B3F-4199-9693-37221C5DA967}");
            }
        }
    }
}