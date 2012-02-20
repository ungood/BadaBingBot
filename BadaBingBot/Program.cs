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
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using BadaBingBot.Api;
using Common.Logging;
using CommonServiceLocator.NinjectAdapter;
using Microsoft.Practices.ServiceLocation;
using Ninject;
using Ninject.Activation;
using Ninject.Extensions.Conventions;
using Topshelf;

namespace BadaBingBot
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            HostFactory.Run(host => {
                host.SetDisplayName("BadaBing Bot");
                host.SetServiceName("BadaBingBot");
                host.SetDescription("A .NET Developer Bot");

                host.Service<RobotProgram>(svc => {
                    svc.ConstructUsing(() => {
                        var kernel = CreateKernel();
                        return kernel.Get<RobotProgram>();
                    });
                    svc.WhenStarted(robot => robot.Run());
                    svc.WhenStopped(robot => robot.Terminate());
                });

                host.RunAsLocalService();
            });
        }

        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel(new RobotNinjaModule());
            var csl = new NinjectServiceLocator(kernel);
            kernel.Bind<IServiceLocator>().ToConstant(csl);
            LoadPluginAssemblies(kernel, kernel.Get<IConfig>());
            return kernel;
        }

        /// <summary>
        /// Load all assemblies in the plugin directory.
        /// </summary>
        private static void LoadPluginAssemblies(IKernel kernel, IConfig config)
        {
            var pluginDir = Path.GetFullPath(config.Plugins.Directory);

            var log = kernel.Get<ILog>();
            log.DebugFormat("Plugins Directory: {0}", pluginDir);

            var pluginAssemblies = new List<Assembly>();
            foreach (var file in Directory.GetFiles(pluginDir, "*.dll"))
            {
                log.DebugFormat("Loading Assembly: {0}", file);
                pluginAssemblies.Add(Assembly.LoadFrom(file));
            }

            kernel.Scan(scanner => {
                scanner.From(pluginAssemblies);
                scanner.AutoLoadModules();
                scanner.WhereTypeInheritsFrom<IPlugin>();
                scanner.BindWith<PluginBindingGenerator<IPlugin>>();
            });
        }

        private class PluginBindingGenerator<TPluginInterface> : IBindingGenerator
        {
            private readonly Type pluginInterfaceType = typeof (TPluginInterface);

            public void Process(Type type, Func<IContext, object> scopeCallback, IKernel kernel)
            {
                if (!pluginInterfaceType.IsAssignableFrom(type))
                    return;
                if (type.IsAbstract || type.IsInterface)
                    return;
                kernel.Bind(pluginInterfaceType).To(type);
            }
        }
    }
}
