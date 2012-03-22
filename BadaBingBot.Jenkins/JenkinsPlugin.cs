using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using BadaBingBot.Api;

namespace BadaBingBot.Jenkins
{
    public class JenkinsPlugin : PluginBase
    {
        public override string Name
        {
            get { return "jenkins"; }
        }

        public override string Description
        {
            get { return "Monitors a jenkins server and publishes build notifications"; }
        }

        public override Uri Url
        {
            get { return new Uri("http://github.com/ungood/badabingbot"); }
        }

        public override IPluginInstance CreateInstance(XElement configElement, IRobot robot)
        {
            var config = DeserializeConfig<JenkinsConfig>(configElement);
            return new JenkinsPluginInstance(config, robot);
        }
    }
}
