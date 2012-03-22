using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace BadaBingBot.Api
{
    public class SubscriptionFilter
    {
        [XmlAttribute("category")]
        public string CategoryPattern { get; set; }

        [XmlAttribute("text")]
        public string TextPattern { get; set; }

        private Regex categoryRegex;

        [XmlIgnore]
        public Regex CategoryRegex
        {
            get
            {
                if(categoryRegex == null)
                    categoryRegex = new Regex(CategoryPattern, RegexOptions.Compiled);

                return categoryRegex;
            }
        }

        private Regex textRegex;

        [XmlIgnore]
        public Regex TextRegex
        {
            get
            {
                if(textRegex == null)
                    textRegex = new Regex(TextPattern, RegexOptions.Compiled);

                return textRegex;
            }
        }

        public bool DoesMessageMatch(IMessage message)
        {
            var categoryMatches = (CategoryPattern == null || CategoryRegex.IsMatch(message.Category));
            var textMatches = (TextPattern == null || TextRegex.IsMatch(message.Text));
            return categoryMatches && textMatches;
        }
    }
}
