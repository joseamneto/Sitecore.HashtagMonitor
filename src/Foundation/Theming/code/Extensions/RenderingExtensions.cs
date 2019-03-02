using Sitecore;
using Sitecore.Mvc.Presentation;

namespace GoHorse.Foundation.Theming.Extensions
{
    public static class RenderingExtensions
    {
        public static string GetBackgroundClass([NotNull] this Rendering rendering)
        {
            return string.IsNullOrEmpty(rendering.Parameters[Constants.CssLayoutParameters.CssClass]) ?
                string.Empty : rendering.Parameters[Constants.CssLayoutParameters.CssClass];
        }
    }
}