using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using BadaBingBot.Api;

namespace BadaBingBot.Chat
{
    public class ChatPlugin : PluginBase
    {
        public override string Name
        {
            get { return "chat"; }
        }

        public override string Description
        {
            get { return "Basic chat commands and natural language processing."; }
        }

        public override Uri Url
        {
            get { return new Uri("http://github.com/ungood/badabingbot"); }
        }

        public override IPluginInstance CreateInstance(XElement configElement, IRobot robot)
        {
            var config = robot.ServiceLocator.GetInstance<IConfig>();
            return new ChatPluginInstance(robot, config.Plugins.Directory, robot.ServiceLocator);
        }
    }
}
