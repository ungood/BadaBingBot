using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BadaBingBot.Api;
using BadaBingBot.Plugin;
using BadaBingBot.Test.Fakes;
using NSubstitute;
using NUnit.Framework;

namespace BadaBingBot.Test
{
    [TestFixture]
    public class PluginManagerTests
    {
        private ILogger logger;
        private IRobot robot;

        [SetUp]
        public void Setup()
        {
            logger = Substitute.For<ILogger>();
            robot = Substitute.For<IRobot>();
        }

        private IConfig SetupConfig(params string[] names)
        {
            var config = Substitute.For<IConfig>();
            var pluginConfig = Substitute.For<IPluginConfig>();

            var pluginInfos = names.Select(name => {
                var info = Substitute.For<IPluginInfo>();
                info.Type.Returns(name);
                return info;
            });

            config.Plugins.Returns(pluginConfig);
            pluginConfig.GetEnumerator().Returns(pluginInfos.GetEnumerator());
            return config;
        }

        [Test]
        public void LoadNoPlugins()
        {
            var manager = new PluginManager(SetupConfig(), logger, Enumerable.Empty<IPlugin>());

            manager.LoadPlugins(robot);
            manager.UnloadPlugins();

            logger.DidNotReceive().Info(Arg.Any<string>(), Arg.Any<object[]>());
        }

        [Test]
        public void LoadingANonExistentPluginShouldLogAnError()
        {
            var config = SetupConfig("fooplugin");
            var manager = new PluginManager(config, logger, Enumerable.Empty<IPlugin>());

            manager.LoadPlugins(robot);

            logger.Received().Error(Arg.Any<string>(), "fooplugin");
        }

        [Test]
        public void SimpleLoadUnloadTest()
        {
            var config = SetupConfig("fooplugin");
            var loadCalled = false;
            var unloadCalled = false;
            var plugins = new[] {
                new FakePlugin("fooplugin", () => loadCalled = true, () => unloadCalled = true)
            };
            var manager = new PluginManager(config, logger, plugins);

            manager.LoadPlugins(robot);
            manager.UnloadPlugins();

            Assert.IsTrue(loadCalled);
            Assert.IsTrue(unloadCalled);
        }

        [Test]
        public void ExceptionDuringLoadShouldLogError()
        {

        }
    }
}
