using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BadaBingBot.Api
{
    public class Message : IEnumerable
    {
        private readonly IDictionary<string, string> properties = new Dictionary<string, string>();

        public string Source { get; set; }

        public string this[string propertyName]
        {
            get
            {
                string value;
                properties.TryGetValue(propertyName, out value);
                return value;
            }
            set { properties[propertyName] = value; }
        }

        public override string ToString()
        {
            return "{ " + string.Join(", ", properties.Select(kvp => kvp.Key + ") : " + kvp.Value)) + " }";
        }

        public Message(string source = null)
        {
            Source = source;
        }

        public IEnumerator GetEnumerator()
        {
            return properties.GetEnumerator();
        }

        public void Add(string name, string value)
        {
            properties.Add(name, value);
        }
    }
}
