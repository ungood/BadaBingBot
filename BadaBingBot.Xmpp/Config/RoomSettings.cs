using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BadaBingBot.Xmpp.Config
{
    public class RoomSettings
    {
        [XmlAttribute("jid")]
        public string Jid { get; set; }

        [XmlAttribute("nickname")]
        public string Nickname { get; set; }

        [XmlAttribute("password")]
        public string Password { get; set; }
    }
}
