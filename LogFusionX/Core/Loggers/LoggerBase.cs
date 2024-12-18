using LogFusionX.Core.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogFusionX.Core.Loggers
{
    public abstract class LoggerBase : LogStructureBase
    {
        public abstract void Log(string message);
        public abstract void Log(string message, Exception? exception, FusionXLoggerLevel fusionXLoggerLevel);
        public abstract void LogCritical(string message, Exception? exception, FusionXLoggerLevel fusionXLoggerLevel);
        public abstract void LogWarning(string message, Exception? exception, FusionXLoggerLevel fusionXLoggerLevel);
        public abstract void LogInfo(string message, Exception? exception, FusionXLoggerLevel fusionXLoggerLevel);
        public abstract void LogDebug(string message, Exception? exception, FusionXLoggerLevel fusionXLoggerLevel);
        public abstract void LogPerformance(string taskName, TimeSpan timeTaken, FusionXLoggerLevel fusionXLoggerLevel);
        public abstract void LogWithTag(string tag, string message, FusionXLoggerLevel fusionXLoggerLevel);
        public abstract void LogError(string message, Exception exception, FusionXLoggerLevel fusionXLoggerLevel);
    }
    public abstract class LogStructureBase
    {
        public abstract void LogStructuredData(string message, Dictionary<string, string> data, FusionXLoggerLevel fusionXLoggerLevel);
        public abstract void LogStructuredData<T>(string message, List<T> data, FusionXLoggerLevel fusionXLoggerLevel);
    }
}
