using System.Collections.Generic;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace BadaBingBot.Api
{
    public class SubscriptionFilter : ConfigurationElement
    {
        private static IDictionary<string, Regex> patterns
            = new Dictionary<string, Regex>();

        [XmlAttribute("sender")]
        public string Sender { get; set; }

        protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
        {
            return base.OnDeserializeUnrecognizedAttribute(name, value);
        }
    }
}
