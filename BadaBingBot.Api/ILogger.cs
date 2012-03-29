using System;

namespace BadaBingBot.Api
{
    /// <summary>
    /// Implementers handle logging and filtering based on logging levels.
    /// </summary>
    public interface ILogger
    {
        bool IsDebugEnabled { get; }
        bool IsInfoEnabled { get; }
        bool IsWarnEnabled { get; }
        bool IsErrorEnabled { get; }
        bool IsFatalEnabled { get; }
        void Debug(Exception ex, string format, params object[] args);
        void Debug(string format, params object[] args);
        void Info(Exception ex, string format, params object[] args);
        void Info(string format, params object[] args);
        void Warn(Exception ex, string format, params object[] args);
        void Warn(string format, params object[] args);
        void Error(Exception ex, string format, params object[] args);
        void Error(string format, params object[] args);
        void Fatal(Exception ex, string format, params object[] args);
        void Fatal(string format, params object[] args);
    }
}