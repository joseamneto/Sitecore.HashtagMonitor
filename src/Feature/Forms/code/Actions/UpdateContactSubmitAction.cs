using GoHorse.Feature.Forms.Models;
using Sitecore.Analytics;
using Sitecore.Diagnostics;
using Sitecore.ExperienceForms.Models;
using Sitecore.ExperienceForms.Processing;
using Sitecore.ExperienceForms.Processing.Actions;
using Sitecore.XConnect;
using Sitecore.XConnect.Client;
using Sitecore.XConnect.Client.Configuration;
using Sitecore.XConnect.Collection.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GoHorse.Feature.Forms.Actions
{

    public class UpdateContact : SubmitActionBase<UpdateContactData>
    {

        public UpdateContact(ISubmitActionData submitActionData) : base(submitActionData)
        {
        }

        protected virtual ITracker CurrentTracker => Tracker.Current;

        protected override bool Execute(UpdateContactData data, FormSubmitContext formSubmitContext)
        {

            Assert.ArgumentNotNull(data, nameof(data));
            Assert.ArgumentNotNull(formSubmitContext, nameof(formSubmitContext));
            var firstNameField = GetFieldById(data.FirstNameFieldId, formSubmitContext.Fields);
            var lastNameField = GetFieldById(data.LastNameFieldId, formSubmitContext.Fields);
            var emailField = GetFieldById(data.EmailFieldId, formSubmitContext.Fields);
            var twitterAccountField = GetFieldById(data.TwitterAccountFieldId, formSubmitContext.Fields);

            var twitterAccount = GetValue(twitterAccountField);
            if (!string.IsNullOrEmpty(twitterAccount))
                Tracker.Current.Session.IdentifyAs("twitter", twitterAccount);

            if (firstNameField == null && lastNameField == null && emailField == null)
            {
                return false;
            }

            using (var client = CreateClient())
            {
                try
                {
                    var trackerIdentifier = new IdentifiedContactReference("twitter", twitterAccount);
                    var expandOptions = new ContactExpandOptions(
                        CollectionModel.FacetKeys.PersonalInformation,
                        CollectionModel.FacetKeys.EmailAddressList);
                    var contact = client.Get(trackerIdentifier, expandOptions);
                    SetPersonalInformation(GetValue(firstNameField), GetValue(lastNameField), GetValue(twitterAccountField), contact, client);
                    SetEmail(GetValue(emailField), contact, client);
                    client.Submit();
                    return true;
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex.Message, ex);
                    return false;
                }
            }
        }

        protected virtual IXdbContext CreateClient()
        {
            return SitecoreXConnectClientConfiguration.GetClient();
        }

        private static IViewModel GetFieldById(Guid id, IList<IViewModel> fields)
        {
            return fields.FirstOrDefault(f => Guid.Parse(f.ItemId) == id);
        }

        private static string GetValue(object field)
        {
            return field?.GetType().GetProperty("Value")?.GetValue(field, null)?.ToString() ?? string.Empty;
        }

        private static void SetPersonalInformation(string firstName, string lastName, string twitterAccount, Contact contact, IXdbContext client)
        {
            if (string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(lastName))
            {
                return;
            }
            PersonalInformation personalInfoFacet = contact.Personal() ?? new PersonalInformation();
            if (personalInfoFacet.FirstName == firstName
                && personalInfoFacet.LastName == lastName
                && personalInfoFacet.Nickname == twitterAccount)
            {
                return;
            }
            personalInfoFacet.FirstName = firstName;
            personalInfoFacet.LastName = lastName;
            personalInfoFacet.Nickname = twitterAccount;
            client.SetPersonal(contact, personalInfoFacet);
        }

        private static void SetEmail(string email, Contact contact, IXdbContext client)
        {
            if (string.IsNullOrEmpty(email))
            {
                return;
            }

            EmailAddressList emailFacet = contact.Emails();

            if (emailFacet == null)
            {
                emailFacet = new EmailAddressList(new EmailAddress(email, false), "Preferred");
            }
            else
            {
                if (emailFacet.PreferredEmail?.SmtpAddress == email)
                {
                    return;
                }
                emailFacet.PreferredEmail = new EmailAddress(email, false);
            }

            client.SetEmails(contact, emailFacet);
        }


    }
}