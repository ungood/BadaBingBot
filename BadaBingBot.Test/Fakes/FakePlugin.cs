using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using BadaBingBot.Api;

namespace BadaBingBot.Test.Fakes
{
    public class FakePlugin : IPlugin
    {
        private readonly Action load;
        private readonly Action unload;
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Uri Url { get; private set; }

        public FakePlugin(string name, Action load = null, Action unload = null)
        {
            this.load = load;
            this.unload = unload;
            Name = name;
            Description = "Fake plugin " + name;
            Url = new Uri("urn://" + name);
        }

        public virtual IPluginInstance CreateInstance(XElement configElement)
        {
            return new FakePluginInstance(load, unload);
        }
    }

    public class FakePluginInstance : IPluginInstance
    {
        private readonly Action load;
        private readonly Action unload;

        public FakePluginInstance(Action load, Action unload)
        {
            this.load = load;
            this.unload = unload;
        }

        public void Load()
        {
            if(load != null)
                load();
        }

        public void Unload()
        {
            if(unload != null)
                unload();
        }
    }
}
