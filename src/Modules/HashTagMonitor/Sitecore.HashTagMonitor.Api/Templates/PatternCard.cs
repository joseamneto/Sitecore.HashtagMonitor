using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Sitecore.HashTagMonitor.Api.Templates
{
    public partial class PatternCard
    {

        /// <summary>
        /// Get Patterns from raw XML value
        /// </summary>
        /// <returns></returns>
        public Dictionary<Guid, double> GetPatterns()
        {
            var ret = new Dictionary<Guid,double>();

            try
            {
                var patternRaw = Pattern;
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(patternRaw);
                var nodes = xmlDoc.SelectNodes("//key");
                if (nodes==null || nodes.Count==0)
                    throw new Exception("No tracking keys found");

                foreach (XmlNode node in nodes)
                {
                    if (node.Attributes == null)
                        continue;

                    // Get profile key from name
                    var name = node.Attributes["name"].Value;
                    var profileKey = GetProfileKey(name);
                    if (profileKey == null)
                        continue;

                    // Cast value to double
                    var value = node.Attributes["value"].Value;
                    double numVal;
                    double.TryParse(value, out numVal);

                    // Add to KeyValuePair
                    ret.Add(profileKey.ID.ToGuid(), numVal);
                }

                return ret;
            }
            catch (Exception e)
            {
                Diagnostics.Log.Error(
                    $"[HashTagMonitor] Cannot get Pattern Keys for Pattern Card '{InnerItem.ID}'", e, this);
                return ret;
            }
        }

        public Profile GetProfile()
        {
            var profileItem = InnerItem.Axes.GetAncestors().FirstOrDefault(p => p.TemplateID == Profile.TemplateID);
            return profileItem == null ? null : new Profile(profileItem);
        }

        /// <summary>
        /// Get ProfileKey given its name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ProfileKey GetProfileKey(string name)
        {
            var profile = InnerItem.Axes.GetAncestors().FirstOrDefault(p => p.TemplateID == Profile.TemplateID);
            var profileKey = profile?.Children.Where(p => p.TemplateID == ProfileKey.TemplateID)
                .Select(p => new ProfileKey(p)).FirstOrDefault(p => p.ProfileKeyName == name);
            return profileKey;
        }
    }
}
