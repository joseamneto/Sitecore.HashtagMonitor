using Sitecore.Data.Items;
using Sitecore.HashTagMonitor.Api.Managers;

namespace Sitecore.HashTagMonitor.Api
{
    public class Task
    {
        private readonly HashTagManager _hashTagManager;
        public Task() : this(new HashTagManager(new Twitter.Twitter())) { }
        public Task(HashTagManager hashTagManager)
        {
            _hashTagManager = hashTagManager;
        }


        public void Execute(Item[] items, Tasks.CommandItem command, Tasks.ScheduleItem schedule)
        {
            _hashTagManager.ProcessAllHashTags();
        }
    }
}
