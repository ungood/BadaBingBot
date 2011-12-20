using System;
using System.Collections.Generic;
using System.Linq;
using BadaBingBot.Api;
using BadaBingBot.Config;
using Common.Logging;

namespace BadaBingBot
{
    public class PluginManager
    {
        private readonly ILog log;
        private readonly IConfig config;
        private readonly IDictionary<string, PluginInfo> plugins; 

        public PluginManager(IConfig config, ILog log, IEnumerable<IPlugin> plugins)
        {
            this.config = config;
            this.log = log;

            this.plugins = plugins
                .ToDictionary(p => p.Name, p => new PluginInfo(p));
        }

        public void LoadPlugins(IRobot robot)
        {
            foreach(var info in plugins.Values)
            {
                log.InfoFormat("Plugin Found: {0} - {1} <{2}>", info.Plugin.Name, info.Plugin.Description, info.Plugin.Url);
            }

            foreach (var pluginElement in config.Plugins)
            {
                PluginInfo info;

                if (!plugins.TryGetValue(pluginElement.Type, out info))
                {
                    log.ErrorFormat("Failed to initialize plugin {0}:  Plugin not found.  Make sure the plugin dll is located in the plugins directory.",
                        pluginElement.Type);
                    continue;
                }

                try
                {
                    log.DebugFormat("Creating and loading instance of plugin {0}", info.Plugin.Name);
                    var instance = info.Plugin.CreateInstance(pluginElement.Config, robot);
                    instance.Load();
                    info.Instances.Add(instance);
                }
                catch (Exception ex)
                {
                    log.ErrorFormat("Unhandled exception while loading instance of plugin {0}",
                        ex, info.Plugin.Name);
                }
            }
        }

        public void UnloadPlugins()
        {
            foreach(var info in plugins.Values)
            {
                log.DebugFormat("Unloading {0} plugin instances", info.Plugin.Name);
                foreach(var instance in info.Instances)
                {
                    instance.Unload();
                }
            }
        }

        private struct PluginInfo
        {
            public IPlugin Plugin { get; set; }
            public IList<IPluginInstance> Instances { get; set; }

            public PluginInfo(IPlugin plugin) : this()
            {
                Plugin = plugin;
                Instances = new List<IPluginInstance>();
            }
        }
    }
}
