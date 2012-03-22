using System.Xml.Serialization;

namespace BadaBingBot.Jenkins
{
    [XmlRoot("jenkins")]
    public class JenkinsConfig
    {
        [XmlElement("server")]
        public ServerSettings[] Servers { get; set; }
    }

    public class ServerSettings
    {
        [XmlAttribute("url")]
        public string Url { get; set; }

        [XmlAttribute("interval")]
        public int PollingInterval { get; set; }
    }
}
