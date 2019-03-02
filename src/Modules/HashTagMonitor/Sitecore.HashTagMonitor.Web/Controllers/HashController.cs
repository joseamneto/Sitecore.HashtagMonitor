using System.Web.Mvc;
using Sitecore.HashTagMonitor.Api.Managers;
using Sitecore.HashTagMonitor.Api.Twitter;

namespace Sitecore.HashTagMonitor.Web.Controllers
{
    public class HashController : Controller
    {
        private readonly HashTagManager _hashTagManager;

        public HashController() : this(new HashTagManager(new Twitter())) { }
        public HashController(HashTagManager hashTagManager)
        {
            _hashTagManager = hashTagManager;
        }

        [HttpGet]
        public ActionResult Process()
        {
            _hashTagManager.ProcessAllHashTags();
            return null;
        }

        [HttpGet]
        public ActionResult AnalyticsRefresh()
        {
            if (Analytics.Tracker.Current == null)
                return null;
            Analytics.Tracker.Current.EndTracking();
            Session.Abandon();
            return null;
        }
    }
}