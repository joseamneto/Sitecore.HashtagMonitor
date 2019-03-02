namespace Sitecore.HashTagMonitor.Api.Templates
{
    public partial class HashTag
    {
        public PatternCard PatternCard => PatternCardReference.TargetItem == null
            ? null
            : new PatternCard(PatternCardReference.TargetItem);

        public Goal Goal => GoalReference.TargetItem == null
            ? null
            : new Goal(GoalReference.TargetItem);
    }
}
