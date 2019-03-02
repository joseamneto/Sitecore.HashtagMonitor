using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.HashTagMonitor.Api.Templates;
using Sitecore.XConnect;
using Sitecore.XConnect.Client;
using Sitecore.XConnect.Collection.Model;
using TweetSharp;
using Goal = Sitecore.HashTagMonitor.Api.Templates.Goal;

namespace Sitecore.HashTagMonitor.Api.Managers
{
    public class HashTagManager
    {
        private readonly Twitter.Twitter _twitter;

        #region Config Settings
        private const string SettingRepositoryPathParam = "HashTagMonitor.RepositoryPath";
        private const string DatabaseParam = "HashTagMonitor.Database";
        private string RepositoryPath =>
            Configuration.Settings.GetSetting(SettingRepositoryPathParam, "/sitecore/system/Modules/HashTagMonitor");
        private string DatabaseName => Configuration.Settings.GetSetting(DatabaseParam, "master");

        private Database _database;
        private Database Database
        {
            get
            {
                if (_database != null)
                    return _database;
                _database = Database.GetDatabase(DatabaseName) ?? Context.Database;
                return _database;
            }
        }

        private Item _repositoryItem;
        private Item RepositoryItem
        {
            get
            {
                if (_repositoryItem != null)
                    return _repositoryItem;
                var repositoryItem = Database.GetItem(RepositoryPath);
                if (repositoryItem != null)
                {
                    _repositoryItem = repositoryItem;
                    return repositoryItem;
                }
                Diagnostics.Log.Error($"[HashTagMonitor] Cannot find repository item '{RepositoryPath}'", this);
                return null;
            }
        }
        #endregion

        public HashTagManager(Twitter.Twitter twitter)
        {
            _twitter = twitter;
        }

        public void ProcessAllHashTags()
        {
            if (RepositoryItem == null)
                return;

            // Get all HashTags
            var hashTagsQuery = RepositoryItem.Axes.GetDescendants()
                .Where(p => p.TemplateID == HashTag.TemplateID);
            var hashTags = hashTagsQuery as Item[] ?? hashTagsQuery.ToArray();
            if (!hashTags.Any())
                return;

            // Process all HashTags
            foreach (var hashTag in hashTags.Select(p => new HashTag(p)))
            {
                var hashTagText = hashTag.Hashtag;
                if (!hashTagText.StartsWith("#"))
                    hashTagText = "#" + hashTagText;

                ProcessHashTag(hashTagText, hashTag.PatternCard, hashTag.Goal);
            }
        }

        public void ProcessHashTag(string hashtag, PatternCard patternCard, Goal goal)
        {
            // Get all tweets from hashtag
            var tweets = _twitter.GetTweets(hashtag, TwitterSearchResultType.Recent, 200);

            foreach (var tweet in tweets)
            {
                // Create Contact + Identifier + Personal Info
                var contact = CreateOrGetContact(hashtag, tweet);
                if (contact == null)
                    continue;

                // Create Interaction
                var isNewInteraction = false;
                var interaction = CreateOrGetInteraction(hashtag, contact, tweet, goal, out isNewInteraction);
                if (interaction == null)
                    continue;

                // Apply Pattern Card to the Contact if the Interaction is new
                if (isNewInteraction && patternCard != null)
                    ApplyPatternCard(contact, tweet, patternCard);
            }
        }

        private void ApplyPatternCard(Contact contact, TwitterStatus tweet, PatternCard patternCard)
        {
            using (var client = XConnect.Client.Configuration.SitecoreXConnectClientConfiguration.GetClient())
            {
                try
                {
                    // Get updated contact with ContactBehaviorProfile facet
                    var updatedContact = client.Get(new IdentifiedContactReference("twitter", tweet.User.ScreenName),
                        new ContactExpandOptions(ContactBehaviorProfile.DefaultFacetKey));
                    if (updatedContact == null)
                        return;

                    // Retrieve facet or create new
                    var isNewFacet = false;
                    var facet =
                        updatedContact.GetFacet<ContactBehaviorProfile>(ContactBehaviorProfile.DefaultFacetKey);
                    if (facet == null) {
                        isNewFacet = true;
                        facet = new ContactBehaviorProfile();
                    }

                    // Change facet properties
                    var score = new ProfileScore {
                        ProfileDefinitionId = patternCard.GetProfile().ID.ToGuid()
                    };
                    if (score.Values==null)
                        score.Values = new Dictionary<Guid, double>();
                    var patterns = patternCard.GetPatterns();
                    foreach (var pair in patterns)
                        score.Values.Add(pair.Key, pair.Value);

                    if (facet.Scores.ContainsKey(patternCard.GetProfile().ID.Guid))
                        facet.Scores[patternCard.GetProfile().ID.Guid] = score;
                    else
                        facet.Scores.Add(patternCard.GetProfile().ID.Guid, score);

                    // Save the facet
                    if (isNewFacet)
                        client.SetFacet<ContactBehaviorProfile>(updatedContact, ContactBehaviorProfile.DefaultFacetKey, facet);
                    else
                        client.SetFacet(updatedContact, ContactBehaviorProfile.DefaultFacetKey, facet);

                    client.Submit();
                }
                catch (XdbExecutionException ex)
                {
                    Diagnostics.Log.Error(
                        $"[HashTagMonitor] Error applying Patter Card to Contact '{contact.Personal().Nickname}'",
                        ex, GetType());
                }
            }
        }

        private Interaction GetInteraction(string hashtag, TwitterStatus tweet, Contact contact)
        {
            Interaction ret = null;
            if (!contact.Id.HasValue)
                return null;

            using (var client = XConnect.Client.Configuration.SitecoreXConnectClientConfiguration.GetClient())
            {
                try
                {
                    var queryable =
                        client.Interactions.Where(x =>
                            x.Contact.Id == contact.Id.Value && x.UserAgent == hashtag &&
                            x.Events.Any(e => e.DataKey == tweet.IdStr));
                    var enumerator = queryable.GetBatchEnumeratorSync(20);
                    while (enumerator.MoveNext())
                        if (enumerator.Current != null)
                            return enumerator.Current.FirstOrDefault();
                }
                catch (XdbExecutionException ex)
                {
                    // Handle exception
                    Diagnostics.Log.Error(
                        $"[HashTagMonitor] Error getting Interaction for contact '{contact.Personal().Nickname}' with hashtag '{hashtag}' and tweetId '{tweet.IdStr}'",
                        ex, GetType());
                }
            }
            return ret;
        }

        private Interaction CreateOrGetInteraction(string hashtag, Contact contact, TwitterStatus tweet, Goal goal, out bool isNewInteraction)
        {
            Interaction interaction = null;
            isNewInteraction = false;
            using (var client = XConnect.Client.Configuration.SitecoreXConnectClientConfiguration.GetClient())
            {
                try
                {
                    // Get interaction if it does exists
                    interaction = GetInteraction(hashtag, tweet, contact);
                    if (interaction != null)
                        return interaction;

                    // Create if does not exists
                    isNewInteraction = true;

                    // Item ID of the "Twitter social community" Channel at 
                    // /sitecore/system/Marketing Control Panel/Taxonomies/Channel/Online/Social community/Twitter social community
                    var channelId = Guid.Parse("{6D3D2374-AF56-44FE-B99A-20843B440B58}");
                    var userAgent = hashtag;
                    interaction = new Interaction(contact, InteractionInitiator.Brand, channelId, userAgent);
                    
                    // Event - Page View
                    var newEvent = new PageViewEvent(tweet.CreatedDate, Guid.Empty, 1, "en")
                    {
                        DataKey = tweet.IdStr,
                        Text = tweet.Text,
                        Url = "https://twitter.com/statuses/" + tweet.IdStr
                    };
                    interaction.Events.Add(newEvent);

                    // Event - Goal
                    if (goal != null)
                    {
                        var newGoal = new XConnect.Goal(goal.ID.Guid, tweet.CreatedDate);
                        interaction.Events.Add(newGoal);
                    }

                    client.AddInteraction(interaction);
                    client.Submit();
                    return interaction;
                }
                catch (XdbExecutionException ex)
                {
                    // Manage exception
                    Diagnostics.Log.Error(
                        $"[HashTagMonitor] Error creating or retrieving interaction for contact '{contact.Personal().Nickname}' with hashtag '{hashtag}' and tweetId '{tweet.IdStr}'",
                        ex, GetType());
                }
            }

            return interaction;
        }

        public byte[] Base64ToByteArray(string base64)
        {
            var imageBytes = System.Convert.FromBase64String(base64);
            using (var ms1 = new MemoryStream(imageBytes))
            {
                var img = Image.FromStream(ms1);
                return ImageToByteArray(img);
            }
        }
        public byte[] ImageToByteArray(Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }
        public bool UpdateContactAvatar(Contact contact, TwitterStatus tweet)
        {
            using (var client = Sitecore.XConnect.Client.Configuration.SitecoreXConnectClientConfiguration.GetClient())
            {
                var avatarUrlBase = "";
                try
                {
                    if (tweet.Entities.Media != null)
                    {
                        var imageUrl = tweet.Entities.Media.FirstOrDefault();
                        avatarUrlBase = imageUrl?.MediaUrlHttps;

                        // Retrieve contact
                        var existingContact = contact;

                        if (existingContact == null)
                            return false;

                        // Retrieve facet (or create one)
                        var facet = existingContact.GetFacet<Avatar>(Avatar.DefaultFacetKey);


                        //get the image 
                        var webClient = new WebClient();
                        byte[] byteArray = webClient.DownloadData(avatarUrlBase);

                        // Change facet properties

                        //var byteArray = Base64ToByteArray(imageBytes);
                        if (facet == null)
                            facet = new Avatar("image/jpeg", byteArray);
                        else
                        {
                            facet.MimeType = "image/jpeg";
                            facet.Picture = byteArray;
                        }

                        client.SetAvatar(existingContact, facet);
                        client.Submit();
                        return true;
                        // Set the updated facet
                    }

                    return false;
                }
                catch (XdbExecutionException ex)
                {
                    // Manage exception
                    Sitecore.Diagnostics.Log.Error(
                        $"[Bookshelf] Error updating avatar.",
                        ex, ex.GetType());
                    return false;
                }
            }
        }
        private Contact CreateOrGetContact(string hashtag, TwitterStatus tweet)
        {
            using (var client = XConnect.Client.Configuration.SitecoreXConnectClientConfiguration.GetClient())
            {
                try
                {
                    var authorAccount = tweet.User.ScreenName;

                    // Get Contact
                    var contactReference = new IdentifiedContactReference("twitter", authorAccount);
                    var contact = client.Get(contactReference, new ExpandOptions { FacetKeys = { "Personal" } });
                    if (contact != null)
                    {
                        UpdateContactAvatar(contact, tweet);
                        return contact;
                    }
                    // Create Contact                    
                    var identifiers = new[] {
                        new ContactIdentifier("twitter", authorAccount, ContactIdentifierType.Known)
                    };
                    var newContact = new Contact(identifiers);

                    UpdateContactAvatar(newContact,tweet);
                

                    // Personal Info
                    var nameSplit = tweet.User.Name.Split(' ');
                    var firstName = nameSplit.Length>0 ? nameSplit[0] : "";
                    var lastName = nameSplit.Length>1 ?  nameSplit[1] : "";
                    var personalInfoFacet = new PersonalInformation
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        Nickname = authorAccount
                    };
                    client.SetFacet<PersonalInformation>(newContact, PersonalInformation.DefaultFacetKey, personalInfoFacet);

                    // E-mail
                    //var emailFacet = new EmailAddressList(new EmailAddress("longhorn@taco.com", true), "twitter");
                    //client.SetFacet<EmailAddressList>(newContact, EmailAddressList.DefaultFacetKey, emailFacet);

                    // Add contact to XConnect
                    client.AddContact(newContact);
                    client.Submit();
                    return CreateOrGetContact(hashtag, tweet);
                }
                catch (XdbExecutionException ex)
                {
                    // Manage exception
                    Diagnostics.Log.Error(
                        $"[HashTagMonitor] Error creating or retrieving contact '{tweet.User.ScreenName}' for hashtag '{hashtag}'",
                        ex, GetType());
                    return null;
                }
            }
        }
    }
}