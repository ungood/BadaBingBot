using System;
using System.Collections.Generic;
using System.Linq;
using BadaBingBot.Api;

namespace BadaBingBot.Plugin
{
    public class PluginManager
    {
        private readonly ILogger logger;
        private readonly IConfig config;
        private readonly IDictionary<string, PluginInfo> plugins;
        
        public IEnumerable<PluginInfo> LoadedPlugins
        {
            get { return plugins.Values; }
        }

        public PluginManager(IConfig config, ILogger logger, IEnumerable<IPlugin> plugins)
        {
            this.config = config;
            this.logger = logger;

            this.plugins = plugins
                .ToDictionary(p => p.Name, p => new PluginInfo(p));
        }

        public void LoadPlugins()
        {
            foreach(var info in plugins.Values)
            {
                logger.Info("Plugin Found: {0} - {1} <{2}>", info.Plugin.Name, info.Plugin.Description, info.Plugin.Url);
            }

            foreach (var pluginElement in config.Plugins)
            {
                PluginInfo info;

                if (!plugins.TryGetValue(pluginElement.Type, out info))
                {
                    logger.Error("Failed to initialize plugin {0}:  Plugin not found.  Make sure the plugin dll is located in the plugins directory.",
                        pluginElement.Type);
                    continue;
                }

                try
                {
                    logger.Info("Creating and loading instance of plugin {0}", info.Plugin.Name);
                    var instance = info.Plugin.CreateInstance(pluginElement.Config);
                    instance.Load();
                    info.Instances.Add(instance);
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Unhandled exception while loading instance of plugin {0}",
                        info.Plugin.Name);
                }
            }
        }

        public void UnloadPlugins()
        {
            foreach(var info in plugins.Values)
            {
                logger.Info("Unloading {0} plugin instances", info.Plugin.Name);
                foreach(var instance in info.Instances)
                {
                    try
                    {
                        instance.Unload();
                    }
                    catch(Exception ex)
                    {
                        logger.Error(ex, "Unhandled exception while unloading instance of plugin {0}",
                            info.Plugin.Name);
                    }
                    
                }
            }

            plugins.Clear();
        }


        public struct PluginInfo
        {
            public IPlugin Plugin { get; private set; }
            public IList<IPluginInstance> Instances { get; set; }

            public PluginInfo(IPlugin plugin) : this()
            {
                Plugin = plugin;
                Instances = new List<IPluginInstance>();
            }
        }
    }
}
