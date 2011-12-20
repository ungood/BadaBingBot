﻿#region License
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

using System.Xml.Linq;
using BadaBingBot.Api;
using BadaBingBot.Xmpp.Config;
using agsXMPP;
using agsXMPP.protocol.x.muc;
using Uri = System.Uri;

namespace BadaBingBot.Xmpp
{
    public class XmppPlugin : PluginBase
    {
        public override string Name
        {
            get { return "xmpp"; }
        }

        public override string Description
        {
            get { return "Allow BadaBingBot to connect to XMPP chat servers."; }
        }

        public override Uri Url
        {
            get { return new Uri("http://github.com/ungood/badabingbot"); }
        }

        public override IPluginInstance CreateInstance(XElement configElement, IRobot robot)
        {
            var config = DeserializeConfig<XmppConfig>(configElement);
            return new XmppPluginInstance(config, robot);
        }
    }
}