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

using System;
using System.ComponentModel.Composition;
using System.Xml.Linq;
using System.Xml.Serialization;
using Common.Logging;

namespace BadaBingBot.Api
{
    [InheritedExport]
    public interface IPlugin
    {
        string Name { get; }
        string Description { get; }
        Uri Url { get; }

        IPluginInstance CreateInstance(XElement configElement);
    }

    public abstract class PluginBase : IPlugin
    {
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract Uri Url { get; }
        public abstract IPluginInstance CreateInstance(XElement configElement);

        protected ILog Log { get; private set; }
        protected IRobot Robot { get; private set; }

        protected PluginBase(IRobot robot, ILog log)
        {
            Log = log;
            Robot = robot;
        }

        protected TConfig DeserializeConfig<TConfig>(XElement configElement)
        {
            var serializer = new XmlSerializer(typeof (TConfig));
            using(var reader = configElement.CreateReader())
            {
               reader.MoveToContent();
               return (TConfig)serializer.Deserialize(configElement.CreateReader());
            }
        }
    }

    public interface IPluginInstance
    {
        void Load();
        void Unload();
    }
}
