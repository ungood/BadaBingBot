using System;
using BadaBingBot.Api;
using Niles.Client;
using Niles.Monitor;

namespace BadaBingBot.Jenkins
{
    public class JenkinsPluginInstance : IPluginInstance
    {
        private readonly JenkinsConfig config;
        private readonly IRobot robot;

        public JenkinsPluginInstance(JenkinsConfig config, IRobot robot)
        {
            this.config = config;
            this.robot = robot;
        }

        public void Load()
        {
            foreach(var server in config.Servers)
            {
                //var client = new JsonJenkinsClient();
                //var monitor = new JenkinsMonitor(client, new RobotJobStateStore());

                robot.ScheduleJob(TimeSpan.FromMilliseconds(server.PollingInterval), () => Poll(server.Url));
            }
        }

        private void Poll(string serverName)
        {
            var text = string.Format("Jenkins build on {0} failed!", serverName);
            robot.Publish(new SimpleMessage(this, "build.failed", text));
        }

        public void Unload()
        {
        }
    }
}
