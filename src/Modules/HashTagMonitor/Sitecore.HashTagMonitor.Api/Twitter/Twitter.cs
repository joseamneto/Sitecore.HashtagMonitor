using System.Collections.Generic;
using TweetSharp;

namespace Sitecore.HashTagMonitor.Api.Twitter
{
    public class Twitter
    {
        public static string ConsumerKey = "jTZHoDGisacVsNoEV3tlyH8uF"; // Add your Key
        public static string ConsumerSecret = "SXmCpmA064MyBsSF6Jrb8io4AAjV6HraYYHpopmz9w1FVdq3Sh"; // Add your Key
        public static string AccessToken = "845015154790223872-oEkmRQMlW3YLObMeGgmPX1JjXoEAN4W"; // Add your Key
        public static string AccessTokenSecret = "E1j98jiZ0sOUwVassZxTUCOSgVcsTtWsBy1H6oJRucxAG"; // Add your Key
        public List<TwitterStatus> GetTweets(string hashtag, TwitterSearchResultType twitterSearchResultType, int count )
        {
            TwitterService twitterService = new TwitterService(ConsumerKey, ConsumerSecret);
            twitterService.AuthenticateWith(AccessToken, AccessTokenSecret);

            int tweetcount = 1;
            //var tweets_search = twitterService.Search(new SearchOptions { Q = "#SCHackathon",Resulttype = TwitterSearchResultType.Popular, Count = 100 });
            var tweets_search = twitterService.Search(new SearchOptions { Q = hashtag, Resulttype = twitterSearchResultType, Count = count });

            //Resulttype can be TwitterSearchResultType.Popular or TwitterSearchResultType.Mixed or TwitterSearchResultType.Recent
            List<TwitterStatus> resultList = new List<TwitterStatus>(tweets_search.Statuses);

            return resultList;
        }
    }
}
