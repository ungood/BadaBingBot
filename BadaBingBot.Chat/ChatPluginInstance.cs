using System;
using AboditNLP;
using BadaBingBot.Api;
using Microsoft.Practices.ServiceLocation;

namespace BadaBingBot.Chat
{
    public class ChatPluginInstance : IPluginInstance
    {
        private readonly IServiceLocator serviceLocator;
        private readonly NLP processor;
        private readonly IDisposable subscription;

        public ChatPluginInstance(IRobot robot, IConfig config, IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
            processor = new NLP(config.Plugins.Directory);

            subscription = robot.Subscribe<IChatMessage>(ExecuteChatMessage);
        }

        public void Load()
        {
            NLP.Debugging = true;
            processor.Initialize();
        }

        public void Unload()
        {
        }

        private void ExecuteChatMessage(IChatMessage message)
        {
            processor.Execute(message.Text, serviceLocator, message);
        }
    }
}
