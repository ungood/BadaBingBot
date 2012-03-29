using System;
using System.Collections.Generic;
using BadaBingBot.Api;
using Ninject.Activation;
using Ninject.Modules;
using log4net;

namespace BadaBingBot.Logging
{
    public class LoggerModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<ILogger>()
                .ToMethod(GetLogger);
        }

        private static ILogger GetLogger(IContext context)
        {
            return GetLogger(context.Request.Target == null
                ? typeof(ILogger)
                : context.Request.Target.Member.DeclaringType);
        }

        private static readonly Dictionary<Type, ILogger> LoggerCache = new Dictionary<Type, ILogger>();

        private static ILogger GetLogger(Type type)
        {
            lock (LoggerCache)
            {
                if (LoggerCache.ContainsKey(type))
                    return LoggerCache[type];

                var log = LogManager.GetLogger(type);
                var logger = new LogWrapper(log);
                LoggerCache.Add(type, logger);

                return logger;
            }
        }
    }
}
