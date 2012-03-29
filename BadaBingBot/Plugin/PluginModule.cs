﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BadaBingBot.Api;
using Ninject;
using Ninject.Extensions.Conventions;
using Ninject.Extensions.Conventions.BindingGenerators;
using Ninject.Modules;
using Ninject.Syntax;

namespace BadaBingBot.Plugin
{
    public class PluginModule<TPluginInterface> : NinjectModule
    {
        public override void Load()
        {
            var config = Kernel.Get<IConfig>();
            var log = Kernel.Get<ILogger>();
            var pluginDirectory = new DirectoryInfo(config.Plugins.Directory);

            log.Debug("Plugins Directory: {0}", pluginDirectory);

            var pluginAssemblies = new List<Assembly>();
            foreach (var file in pluginDirectory.GetFiles("*.dll"))
            {
                log.Debug("Loading Assembly: {0}", file);
                try
                {
                    pluginAssemblies.Add(Assembly.LoadFrom(file.FullName));
                }
                catch(Exception ex)
                {
                    log.Error("Failed to load assembly " + file, ex);
                }
            }

            Kernel.Load(pluginAssemblies);
           
            Kernel.Bind(scanner => scanner
                .From(pluginAssemblies)
                .SelectAllClasses()
                .InheritedFrom<IPlugin>()
                .BindWith<PluginBindingGenerator>()
            );
        }

        private class PluginBindingGenerator : IBindingGenerator
        {
            public IEnumerable<IBindingWhenInNamedWithOrOnSyntax<object>> CreateBindings(Type type, Ninject.Syntax.IBindingRoot bindingRoot)
            {
                Type pluginInterfaceType = typeof(TPluginInterface);

                if (!pluginInterfaceType.IsAssignableFrom(type))
                    yield break;
                if (type.IsAbstract || type.IsInterface)
                    yield break;

                yield return bindingRoot.Bind(pluginInterfaceType).To(type);
            }
        }
    }
}