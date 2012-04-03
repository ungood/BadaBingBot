using BadaBingBot.Api;
using BadaBingBot.Plugin;

namespace BadaBingBot
{
    public class RobotProgram
    {
        private ILogger logger;
        private IConfig config;
        private IRobot robot;
        private readonly PluginManager pluginManager;

        public RobotProgram(ILogger logger, IConfig config, IRobot robot, PluginManager pluginManager)
        {
            this.logger = logger;
            this.config = config;
            this.robot = robot;
            this.pluginManager = pluginManager;
        }

        public void Run()
        {
            logger.Info("{0} Powering UP!", config.Name);
            pluginManager.LoadPlugins();
        }

        public void Terminate()
        {
            pluginManager.UnloadPlugins();
            logger.Info("{0} has been terminated.", config.Name);
        }
    }
}
