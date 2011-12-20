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

using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;
using System.Xml.Linq;
using BadaBingBot.Api;

namespace BadaBingBot.Config
{
    public class PluginElement : ConfigurationElement, IPluginConfig
    {
        [ConfigurationProperty("directory", DefaultValue=".\\Plugins")]
        public string Directory
        {
            get { return (string) base["directory"]; }
            set { base["directory"] = value; }
        }

        private readonly IList<IPluginInfo> plugins = new List<IPluginInfo>(); 

        protected override bool OnDeserializeUnrecognizedElement(string elementName, XmlReader reader)
        {
            var element = (XElement) XNode.ReadFrom(reader);
            plugins.Add(new PluginInfo {Config = element, Type = elementName});
            return true;
        }

        public IEnumerator<IPluginInfo> GetEnumerator()
        {
            return plugins.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class PluginInfo : IPluginInfo
    {
        public string Type { get; set; }
        public XElement Config { get; set; }
    }
}
