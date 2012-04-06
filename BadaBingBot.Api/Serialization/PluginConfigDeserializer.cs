using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace BadaBingBot.Api.Serialization
{
    public class PluginConfigSerializer<TConfig>
    {
        private readonly XmlSerializer serializer;

        public PluginConfigSerializer()
        {
            serializer = new XmlSerializer(typeof(TConfig));
            serializer.UnknownAttribute += SerializerOnUnknownAttribute;
        }

        public TConfig Deserializer(XElement element)
        {
            using (var reader = element.CreateReader())
            {
                reader.MoveToContent();
                return (TConfig)serializer.Deserialize(configElement.CreateReader());
            }
        }

        private void SerializerOnUnknownAttribute(object sender, XmlAttributeEventArgs e)
        {
            var handler = e.ObjectBeingDeserialized as IHandleUnknownAttributes;
            if(handler == null)
                return;

            handler.Add(e.Attr.Name, e.Attr.Value);
        }
    }
}