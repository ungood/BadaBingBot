using System;
using System.Linq;
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
        
        [SetUp]
        public void Setup()
        {
            logger = Substitute.For<ILogger>();
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

            manager.LoadPlugins();
            manager.UnloadPlugins();

            logger.DidNotReceive().Info(Arg.Any<string>(), Arg.Any<object[]>());
        }

        [Test]
        public void LoadingANonExistentPluginShouldLogAnError()
        {
            var config = SetupConfig("fooplugin");
            var manager = new PluginManager(config, logger, Enumerable.Empty<IPlugin>());

            manager.LoadPlugins();

            logger.Received().Error(Arg.Any<string>(), "fooplugin");
            Assert.AreEqual(0, manager.LoadedPlugins.Count());
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

            manager.LoadPlugins();
            Assert.IsTrue(loadCalled);
            Assert.AreEqual(1, manager.LoadedPlugins.Count());
            Assert.AreEqual(1, manager.LoadedPlugins.First().Instances.Count());

            manager.UnloadPlugins();
            Assert.IsTrue(unloadCalled);
            Assert.AreEqual(0, manager.LoadedPlugins.Count());
        }

        [Test]
        public void ExceptionDuringCreateInstanceShouldLogError()
        {
            var config = SetupConfig("fooplugin");
            var plugins = new[] {
                new ThrowingCreateInstancePlugin("fooplugin"), 
            };
            var manager = new PluginManager(config, logger, plugins);

            manager.LoadPlugins();
            logger.Received().Error(Arg.Any<Exception>(), Arg.Any<string>(), "fooplugin");
            Assert.AreEqual(1, manager.LoadedPlugins.Count());
            Assert.AreEqual(0, manager.LoadedPlugins.First().Instances.Count());

            manager.UnloadPlugins();
        }

        [Test]
        public void ExceptionDuringLoadShouldLogError()
        {
            var config = SetupConfig("fooplugin");
            var plugins = new[] {
                new ThrowingLoadPlugin("fooplugin")
            };
            var manager = new PluginManager(config, logger, plugins);

            manager.LoadPlugins();
            logger.Received().Error(Arg.Any<Exception>(), Arg.Any<string>(), "fooplugin");
            Assert.AreEqual(1, manager.LoadedPlugins.Count());
            Assert.AreEqual(0, manager.LoadedPlugins.First().Instances.Count());

            manager.UnloadPlugins();
        }

        [Test]
        public void ExceptionDuringUnloadShouldLogError()
        {
            var config = SetupConfig("fooplugin");
            var plugins = new[] {
                new ThrowingUnloadPlugin("fooplugin")
            };
            var manager = new PluginManager(config, logger, plugins);

            manager.LoadPlugins();
            logger.DidNotReceive().Error(Arg.Any<Exception>(), Arg.Any<string>(), "fooplugin");

            manager.UnloadPlugins();
            logger.Received().Error(Arg.Any<Exception>(), Arg.Any<string>(), "fooplugin");
        }
    }
}
