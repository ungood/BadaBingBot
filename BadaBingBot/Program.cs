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
using System.IO;
using BadaBingBot.Api;
using BadaBingBot.Logging;
using BadaBingBot.Plugin;
using Ninject;
using Topshelf;

namespace BadaBingBot
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo("log4net.Config"));

            var host = HostFactory.New(cfg => {
                cfg.SetDisplayName("BadaBing Bot");
                cfg.SetServiceName("BadaBingBot");
                cfg.SetDescription("A .NET Developer Bot");
                cfg.Service<RobotProgram>(svc => {
                    svc.ConstructUsing(() => {
                        var kernel = CreateKernel();
                        return kernel.Get<RobotProgram>();
                    });
                    svc.WhenStarted(robot => robot.Run());
                    svc.WhenStopped(robot => robot.Terminate());
                });

                cfg.RunAsLocalService();
            });
            
            host.Run();
            if(Environment.UserInteractive)
            {
                Console.WriteLine("Press any key..");
                Console.ReadKey(true);
            }
        }

        private static IKernel CreateKernel()
        {
            try
            {
                var kernel = new StandardKernel(
                    new RobotNinjaModule(),
                    new LoggerModule(),
                    new PluginModule<IPlugin>());
                return kernel;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            
        }
    }
}
