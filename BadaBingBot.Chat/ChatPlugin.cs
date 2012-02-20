using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using BadaBingBot.Api;

namespace BadaBingBot.Chat
{
    public class ChatPlugin : IPlugin
    {
        public string Name
        {
            get { return "chat"; }
        }

        public string Description
        {
            get { return "Basic chat commands and natural language processing."; }
        }

        public Uri Url
        {
            get { return new Uri("http://github.com/ungood/badabingbot"); }
        }

        public IPluginInstance CreateInstance(XElement configElement, IRobot robot)
        {
            var config = robot.ServiceLocator.GetInstance<IConfig>();
            return new ChatPluginInstance(robot, config.Plugins.Directory, robot.ServiceLocator);
        }
    }
}
