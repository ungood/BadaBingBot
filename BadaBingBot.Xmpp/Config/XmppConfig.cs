#region License
// Copyright 2011 Jason Walker
// ungood@onetrue.name
// 
// Licensed under the Apache License, Version 2.0 (the "License"); 
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS, 
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and 
// limitations under the License.
#endregion

using System.Xml.Serialization;

namespace BadaBingBot.Xmpp.Config
{
    [XmlRoot("xmpp")]
    public class XmppConfig
    {
        [XmlAttribute("resource")]
        public string Resource { get; set; }

        [XmlAttribute("server")]
        public string Server { get; set; }

        [XmlAttribute("port")]
        public int Port { get; set; }

        [XmlAttribute("username")]
        public string Username { get; set; }

        [XmlAttribute("password")]
        public string Password { get; set; }

        [XmlAttribute("useSSL")]
        public bool UseSSL { get; set; }

        [XmlAttribute("useStartTLS")]
        public bool UseStartTLS { get; set; }

        [XmlElement("room")]
        public RoomSettings[] Rooms { get; set; }

        public XmppConfig()
        {
            Resource = "BadaBingBot";
            Port = 5222;
            UseStartTLS = true;
        }
    }
}
