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
using System.Collections.Specialized;
using System.Configuration;
using BadaBingBot.Api;
using BadaBingBot.Config;
using Common.Logging;
using Common.Logging.Log4Net;
using Common.Logging.Simple;
using Ninject.Activation;
using Ninject.Modules;

namespace BadaBingBot
{
    public class RobotNinjaModule : NinjectModule
    {
        public override void Load()
        {
            var logConfig = new NameValueCollection();
            logConfig["configType"] = "FILE-WATCH";
            logConfig["configFile"] = "~/log4net.config";
            LogManager.Adapter = new Log4NetLoggerFactoryAdapter(logConfig);
            Bind<ILog>()
                .ToMethod(GetLogger);

            var config = (RobotSection)ConfigurationManager.GetSection("robot");
            Bind<IConfig>()
                .ToConstant(config);

            Bind<PluginManager>()
                .To<PluginManager>()
                .InSingletonScope();

            Bind<IRobot>()
                .To<Robot>()
                .InSingletonScope();
        }

        private static ILog GetLogger(IContext context)
        {
            return LogManager.GetLogger(context.GetType());
        }
    }
}
