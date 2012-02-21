using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using BadaBingBot.Api;
using Common.Logging;

namespace BadaBingBot.Chat
{
    public class ChatPlugin : IPlugin
    {
        private readonly IConfig config;
        private readonly IRobot robot;
        private readonly ILog log;

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
            return new ChatPluginInstance(robot, robot.Config.Plugins.Directory, robot.ServiceLocator);
        }
    }
}
