using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using BadaBingBot.Api;
using Common.Logging;

namespace BadaBingBot
{
    public class RobotProgram
    {
        private ILog log;
        private IConfig config;
        private IRobot robot;
        private readonly PluginManager pluginManager;

        public RobotProgram(ILog log, IConfig config, IRobot robot, PluginManager pluginManager)
        {
            this.log = log;
            this.config = config;
            this.robot = robot;
            this.pluginManager = pluginManager;
        }

        public void Run()
        {
            log.InfoFormat("{0} Powering UP!", config.Name);
            pluginManager.LoadPlugins(robot);
        }

        public void Terminate()
        {
            pluginManager.UnloadPlugins();
            log.InfoFormat("{0} has been terminated.", config.Name);
        }
    }
}
