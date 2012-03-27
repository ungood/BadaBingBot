using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BadaBingBot.Api;

namespace BadaBingBot.Log4Net
{
    public class Log4NetLog : ILog
    {
        private log4net.ILog log;

        public Log4NetLog(log4net.ILog log)
        {
            this.log = log;
        }

        bool ILog.IsDebugEnabled
        {
            get { return log.IsDebugEnabled; }
        }

        bool ILog.IsInfoEnabled
        {
            get { return log.IsInfoEnabled; }
        }

        bool ILog.IsWarnEnabled
        {
            get { return log.IsWarnEnabled; }
        }

        bool ILog.IsErrorEnabled
        {
            get { return log.IsErrorEnabled; }
        }

        bool ILog.IsFatalEnabled
        {
            get { return log.IsFatalEnabled; }
        }

        void ILog.Debug(object obj)
        {
            log.Debug(obj);
        }

        void ILog.Debug(object obj, Exception exception)
        {
            log.Debug(obj, exception);
        }

        void ILog.DebugFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            log.DebugFormat(formatProvider, format, args);
        }

        void ILog.DebugFormat(string format, params object[] args)
        {
            log.DebugFormat(format, args);
        }

        void ILog.Info(object obj)
        {
            log.Info(obj);
        }

        void ILog.Info(object obj, Exception exception)
        {
            log.Info(obj, exception);
        }

        void ILog.InfoFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            log.InfoFormat(formatProvider, format, args);
        }

        void ILog.InfoFormat(string format, params object[] args)
        {
            log.InfoFormat(format, args);
        }

        void ILog.Warn(object obj)
        {
            log.Warn(obj);
        }

        void ILog.Warn(object obj, Exception exception)
        {
            log.Warn(obj, exception);
        }

        void ILog.WarnFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            log.WarnFormat(formatProvider, format, args);
        }

        void ILog.WarnFormat(string format, params object[] args)
        {
            log.WarnFormat(format, args);
        }

        void ILog.Error(object obj)
        {
            log.Error(obj);
        }

        void ILog.Error(object obj, Exception exception)
        {
            log.Error(obj, exception);
        }

        void ILog.ErrorFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            log.ErrorFormat(formatProvider, format, args);
        }

        void ILog.ErrorFormat(string format, params object[] args)
        {
            log.ErrorFormat(format, args);
        }

        void ILog.Fatal(object obj)
        {
            log.Fatal(obj);
        }

        void ILog.Fatal(object obj, Exception exception)
        {
            log.Fatal(obj, exception);
        }

        void ILog.FatalFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            log.FatalFormat(formatProvider, format, args);
        }

        void ILog.FatalFormat(string format, params object[] args)
        {
            log.FatalFormat(format, args);
        }
    }
}
