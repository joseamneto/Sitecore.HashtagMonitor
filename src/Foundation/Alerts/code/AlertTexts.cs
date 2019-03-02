using Sitecore.Data;

namespace GoHorse.Foundation.Alerts
{
    public class AlertTexts
    {
        public static string InvalidDataSourceTemplate(ID templateId) =>
            string.Format("Data source isn't set or have wrong template. Template {0} is required", templateId);

        public static string InvalidDataSourceTemplateFriendlyMessage =>
            ("There was a problem with the associated content item, please associate a correct content item with the component");

        public static string InvalidDataSource =>
            ("Data source isn't set or have wrong template");
    }
}