using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using BadaBingBot.Api;

namespace BadaBingBot.Test.Fakes
{
    public class ThrowingCreateInstance : FakePlugin
    {
        public ThrowingCreateInstance(string name)
            : base(name)
        {
        }

        public override IPluginInstance CreateInstance(XElement configElement)
        {
            throw new NotImplementedException();
        }
    }

    public class ThrowingLoadPlugin : FakePlugin
    {
        public ThrowingLoadPlugin(string name)
            : base(name, () => {
                throw new NotImplementedException();
            })

        {
        }
    }

    public class ThrowingUnloadPlugin : FakePlugin
    {
        public ThrowingUnloadPlugin(string name)
            : base(name, () => {
                throw new NotImplementedException();
            })

        {
        }
    }
}
