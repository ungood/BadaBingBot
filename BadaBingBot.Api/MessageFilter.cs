using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using BadaBingBot.Api.Serialization;

namespace BadaBingBot.Api
{
    public class MessageFilter : IHandleUnknownAttributes, IEnumerable
    {
        private readonly IDictionary<string, Regex> patterns
            = new Dictionary<string, Regex>();

        [XmlAttribute("source")]
        public string Source { get; set; }

        public void Add(string attributeName, string attributeValue)
        {
            var pattern = new Regex(attributeValue);
            patterns.Add(attributeName, pattern);
        }

        public bool Matches(Message message)
        {
            return SourceMatches(message) && PatternsMatch(message);
        }

        private bool SourceMatches(Message message)
        {
            if (Source == null)
                return true;

            return Source == message.Source;
        }

        private bool PatternsMatch(Message message)
        {
            return patterns.All(kvp => PatternMatches(message, kvp.Key, kvp.Value));
        }

        private bool PatternMatches(Message message, string name, Regex pattern)
        {
            var value = message[name];
            if (value == null)
                return false;

            return pattern.IsMatch(value);
        }

        public IEnumerator GetEnumerator()
        {
            return patterns.GetEnumerator();
        }
    }
}
