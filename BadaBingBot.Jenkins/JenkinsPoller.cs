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
using System.Linq;
using System.Text;
using BadaBingBot.Api;
using Niles.Monitor;

namespace BadaBingBot.Jenkins
{
    public class JenkinsPoller
    {
        private readonly ILogger logger;
        private readonly IRobot robot;
        private readonly TimeSpan interval;
        private readonly JenkinsMonitor monitor;

        public JenkinsPoller(IRobot robot, ServerSettings settings, ILogger logger)
        {
            this.robot = robot;
            this.logger = logger;
            interval = TimeSpan.FromMilliseconds(settings.PollingInterval);
            monitor = new JenkinsMonitor(new Uri(settings.Url));

            monitor.PollingError += OnPollingError;
            monitor.BuildStarted += OnBuildStarted;
            monitor.BuildAborted += (sender, e) => PublishBuild(e, "abort");
            monitor.BuildFailed += (sender, e) => PublishBuild(e, "fail");
            monitor.BuildSucceeded += (sender, e) => PublishBuild(e, "succeed");
            monitor.BuildUnstable += (sender, e) => PublishBuild(e, "fail");
        }

        public void StartPolling()
        {
            robot.ScheduleJob(interval, Poll);
        }

        private void Poll()
        {
            try
            {
                logger.Debug("Polling: {0}", monitor.BaseUri);
                monitor.Poll(5000);
            }
            catch(TimeoutException)
            {
            }
        }

        private void OnPollingError(object sender, PollingErrorEventArgs e)
        {
            logger.Error("Error occured while polling", e.Exception);
        }

        private void OnBuildStarted(object sender, BuildEventArgs e)
        {
            var message = new BuildMessage(this) {
                JobName = e.Job.Name,
                Result = "started",
                Text = e.Job + " started.",
            };

            robot.Publish(message);
        }

        private void PublishBuild(BuildEventArgs e, string verb)
        {
            var tense = e.StatusChanged
                ? verb + "ed"
                : verb + "ing";

            var text = new StringBuilder();
            text.AppendFormat("{0} {1}{2}!",
                e.Job.Name,
                e.StatusChanged ? "" : "still ",
                tense);

            if(e.Build.Culprits != null)
            {
                var culprits = e.Build.Culprits.Select(u => u.FullName);
                text.AppendFormat(" Culprits: {0}", string.Join(", ", culprits));
            }

            var message = new BuildMessage(this) {
                JobName = e.Job.Name,
                Result = tense,
                Text = text.ToString()
            };

            robot.Publish(message);
        }
    }
}
