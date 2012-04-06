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

namespace BadaBingBot.Api
{
    public interface IPlugin
    {
        string Name { get; }
        string Description { get; }
        Uri Url { get; }

        void Load();
        void Unload();
    }

    public interface IPlugin<in TConfig> : IPlugin
    {
        void Configure(TConfig config);
    }

    public abstract class Plugin : IPlugin
    {
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract Uri Url { get; }
        
        public virtual void Load()
        {
        }

        public virtual void Unload()
        {
        }

        protected ILogger Logger { get; private set; }
        protected IRobot Robot { get; private set; }

        protected Plugin(IRobot robot, ILogger logger)
        {
            Logger = logger;
            Robot = robot;
        }
    }

    public abstract class Plugin<TConfig> : Plugin, IPlugin<TConfig>
    {
        protected TConfig Config { get; private set; }

        protected Plugin(IRobot robot, ILogger logger)
            : base(robot, logger)
        {
        }

        public void Configure(TConfig config)
        {
            Config = config;
        }
    }
}
