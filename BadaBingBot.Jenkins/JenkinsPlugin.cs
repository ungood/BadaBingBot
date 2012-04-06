#region License
// Copyright 2012 Jason Walker
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
using BadaBingBot.Api;

namespace BadaBingBot.Jenkins
{
    public class JenkinsPlugin : Plugin<JenkinsConfig>
    {
        public JenkinsPlugin(IRobot robot, ILogger logger)
            : base(robot, logger)
        {
        }

        public override string Name
        {
            get { return "jenkins"; }
        }

        public override string Description
        {
            get { return "Monitors a jenkins server and publishes build notifications"; }
        }

        public override Uri Url
        {
            get { return new Uri("http://github.com/ungood/badabingbot"); }
        }

        public override void Load()
        {
            foreach(var server in Config.Servers)
            {
                var interval = TimeSpan.FromMilliseconds(server.PollingInterval);
                var poller = new JenkinsPoller(server.Url, Robot, Logger);
                Robot.ScheduleJob(interval, poller.Poll);
            }
        }
    }
}
