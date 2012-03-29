using System;
using BadaBingBot.Api;
using log4net;

namespace BadaBingBot.Logging
{
    public class LogWrapper : ILogger
    {
        private readonly ILog log;

        public LogWrapper(ILog log)
        {
            this.log = log;
        }

        public bool IsDebugEnabled
        {
            get { return log.IsDebugEnabled; }
        }

        public bool IsInfoEnabled
        {
            get { return log.IsInfoEnabled; }
        }

        public bool IsWarnEnabled
        {
            get { return log.IsWarnEnabled; }
        }

        public bool IsErrorEnabled
        {
            get { return log.IsErrorEnabled; }
        }

        public bool IsFatalEnabled
        {
            get { return log.IsFatalEnabled; }
        }

        public void Debug(Exception ex, string format, params object[] args)
        {
            log.DebugFormat(string.Format(format, args), ex);
        }

        public void Debug(string format, params object[] args)
        {
            log.DebugFormat(format, args);
        }

        public void Info(Exception ex, string format, params object[] args)
        {
            log.Info(string.Format(format, args), ex);
        }

        public void Info(string format, params object[] args)
        {
            log.InfoFormat(format, args);
        }

        public void Warn(Exception ex, string format, params object[] args)
        {
            log.WarnFormat(string.Format(format, args), ex);
        }

        public void Warn(string format, params object[] args)
        {
            log.WarnFormat(format, args);
        }

        public void Error(Exception ex, string format, params object[] args)
        {
            log.ErrorFormat(string.Format(format, args), ex);
        }

        public void Error(string format, params object[] args)
        {
            log.ErrorFormat(format, args);
        }

        public void Fatal(Exception ex, string format, params object[] args)
        {
            log.Fatal(string.Format(format, args), ex);
        }

        public void Fatal(string format, params object[] args)
        {
            log.FatalFormat(format, args);
        }
    }
}
