using System;
using AboditNLP;
using BadaBingBot.Api;
using Microsoft.Practices.ServiceLocation;

namespace BadaBingBot.Chat
{
    public class ChatPluginInstance : IPluginInstance
    {
        private readonly IRobot robot;
        private readonly IServiceLocator serviceLocator;
        private readonly NLP processor;

        public ChatPluginInstance(IRobot robot, string pluginsDirectory, IServiceLocator serviceLocator)
        {
            this.robot = robot;
            this.serviceLocator = serviceLocator;
            processor = new NLP(pluginsDirectory);

            robot.Subscribe<IChatMessage>(ExecuteChatMessage);
        }

        public void Load()
        {
            NLP.Debugging = true;
            processor.Initialize();
            foreach (var rule in NLP.DumpRules())
                robot.Log.Debug(rule);
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
