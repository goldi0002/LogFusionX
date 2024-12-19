using LogFusionX.Core.Configurations;
using LogFusionX.Core.Utils;
using LogFusionX.FileWriter;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace LogFusionX.Core.Loggers
{
    public class XLogger : LoggerBase
    {
        private readonly string _xLogFilePath;
        private readonly XFileLoggerWriterAdvanced _writer;
        private readonly XLoggerFormat xLoggerFormat;
        public XLogger(string _xFilePath, string _fileName, int MaxFileSizeInMB = 10)
        {
            if (string.IsNullOrWhiteSpace(_xFilePath)) throw new ArgumentNullException(nameof(_xFilePath));
            _xLogFilePath = _xFilePath;
            _writer = new XFileLoggerWriterAdvanced(_xLogFilePath, _fileName, MaxFileSizeInMB);
            xLoggerFormat = new XLoggerFormat(null);
        }
        public XLogger(XFileLoggerConfigurationOptions xFileLoggerConfigurationOptions)
        {
            if (string.IsNullOrWhiteSpace(xFileLoggerConfigurationOptions.LogDirectory)) throw new ArgumentNullException(nameof(xFileLoggerConfigurationOptions.LogDirectory));
            _xLogFilePath = xFileLoggerConfigurationOptions.LogDirectory;
            _writer = new XFileLoggerWriterAdvanced(xFileLoggerConfigurationOptions);
            xLoggerFormat = new XLoggerFormat(xFileLoggerConfigurationOptions.DateFormat);
        }
        #region "Private Members"
        private static string GetCurrentMethodFullName()
        {
            var stackTrace = new StackTrace();
            var frame = stackTrace.GetFrame(2);
            var method = frame?.GetMethod();
            if (method != null)
            {
                var className = method.DeclaringType?.FullName;
                var methodName = method.Name;
                return $"{className}.{methodName}";
            }
            return "Unknown Method";
        }
        #endregion
        public override void Log(string message)
        {
            _writer.WriteLog(message);
        }

        public override void Log(string message, Exception? exception, FusionXLoggerLevel fusionXLoggerLevel, XLoggerFormats xLogFormat)
        {
            if (exception != null)
                commonWriter(message, exception, fusionXLoggerLevel, xLogFormat);
            else
                Log(xLoggerFormat.GetLogFormat(FusionXLoggerLevel.None, message, null, GetCurrentMethodFullName(), xLogFormat));
        }
        private void commonWriter(string message, Exception? exception, FusionXLoggerLevel fusionXLoggerLevel, XLoggerFormats xLogFormat)
        {
            _writer.WriteLog(xLoggerFormat.GetLogFormat(fusionXLoggerLevel, message, exception, GetCurrentMethodFullName(), xLogFormat));
        }
        public override void LogCritical(string message, Exception? exception, FusionXLoggerLevel fusionXLoggerLevel, XLoggerFormats xLogFormat)
        {
            commonWriter(message, exception, fusionXLoggerLevel, xLogFormat);
        }

        public override void LogWarning(string message, Exception? exception, FusionXLoggerLevel fusionXLoggerLevel, XLoggerFormats xLogFormat)
        {
            commonWriter(message, exception, fusionXLoggerLevel, xLogFormat);
        }

        public override void LogInfo(string message, Exception? exception, FusionXLoggerLevel fusionXLoggerLevel, XLoggerFormats xLogFormat)
        {
            commonWriter(message, exception, fusionXLoggerLevel, xLogFormat);
        }

        public override void LogDebug(string message, Exception? exception, FusionXLoggerLevel fusionXLoggerLevel, XLoggerFormats xLogFormat)
        {
            commonWriter(message, exception, fusionXLoggerLevel, xLogFormat);
        }

        public override void LogPerformance(string taskName, TimeSpan timeTaken, FusionXLoggerLevel fusionXLoggerLevel, XLoggerFormats xLogFormat)
        {
            throw new NotImplementedException();
        }

        public override void LogWithTag(string tag, string message, FusionXLoggerLevel fusionXLoggerLevel, XLoggerFormats xLogFormat)
        {
            throw new NotImplementedException();
        }

        public override void LogError(string message, Exception exception, FusionXLoggerLevel fusionXLoggerLevel, XLoggerFormats xLogFormat)
        {
            commonWriter(message, exception, fusionXLoggerLevel, xLogFormat);
        }

        public override void LogStructuredData(string message, Dictionary<string, string> data, FusionXLoggerLevel fusionXLoggerLevel)
        {
            throw new NotImplementedException();
        }

        public override void LogStructuredData<T>(string message, List<T> data, FusionXLoggerLevel fusionXLoggerLevel)
        {
            throw new NotImplementedException();
        }
    }
}
