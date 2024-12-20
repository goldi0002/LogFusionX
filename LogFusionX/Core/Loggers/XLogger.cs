using LogFusionX.Core.Configurations;
using LogFusionX.Core.Utils;
using LogFusionX.FileWriter;
using LogFusionX.StructuredLogWriter;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace LogFusionX.Core.Loggers
{
    public class XLogger : LoggerBase
    {
        private readonly string _xLogFilePath;
        private readonly XFileLoggerWriterAdvanced _writer;
        private readonly XLoggerFormat _xLoggerFormat;
        private readonly FusionXConsoleLogger _xConsoleLogger;
        private readonly bool _isConsoleLoggingEnabled;
        private readonly XFusionXMLWriter _xFusionXMLWriter;
        public XLogger(string xFilePath, string fileName, int maxFileSizeInMB = 10)
        {
            if (string.IsNullOrWhiteSpace(xFilePath)) throw new ArgumentNullException(nameof(xFilePath));

            _xLogFilePath = xFilePath;
            _writer = new XFileLoggerWriterAdvanced(_xLogFilePath, fileName, maxFileSizeInMB);
            _xLoggerFormat = new XLoggerFormat();
            _xConsoleLogger = new FusionXConsoleLogger();
            _isConsoleLoggingEnabled = false;
            _xFusionXMLWriter = new XFusionXMLWriter(Path.Combine(xFilePath, fileName));
        }

        public XLogger(XLoggerConfigurationOptions configOptions)
        {
            if (string.IsNullOrWhiteSpace(configOptions.LogDirectory)) throw new ArgumentNullException(nameof(configOptions.LogDirectory));

            _xLogFilePath = configOptions.LogDirectory;
            _writer = new XFileLoggerWriterAdvanced(configOptions);
            _xLoggerFormat = new XLoggerFormat(configOptions.DateFormat);
            _xConsoleLogger = new FusionXConsoleLogger();
            _isConsoleLoggingEnabled = configOptions.EnableConsoleLogging;
            _xFusionXMLWriter = new XFusionXMLWriter(Path.Combine(configOptions.LogDirectory, configOptions.LogFileName));
        }

        #region Private Methods

        private static string GetCurrentMethodFullName()
        {
            var stackTrace = new StackTrace();
            var frame = stackTrace.GetFrame(2);
            var method = frame?.GetMethod();
            return method != null ? $"{method.DeclaringType?.FullName}.{method.Name}" : "Unknown Method";
        }

        private void WriteToConsole(string message, Exception? exception, FusionXLoggerLevel level, XLoggerFormats format)
        {
            var logMessage = _xLoggerFormat.GetLogFormat(level, message, exception, GetCurrentMethodFullName(), format);
            _xConsoleLogger.Log(logMessage, level);
        }

        private void WriteToFile(string message, Exception? exception, FusionXLoggerLevel level, XLoggerFormats format)
        {
            var logMessage = _xLoggerFormat.GetLogFormat(level, message, exception, GetCurrentMethodFullName(), format);
            _writer.EnqueueLog(logMessage);
        }

        #endregion

        #region Public Methods

        public override void Log(string message)
        {
            if (_isConsoleLoggingEnabled)
            {
                WriteToConsole(message, null, FusionXLoggerLevel.Info, XLoggerFormats.StandardLogFormat);
            }
            else
            {
                WriteToFile(message, null, FusionXLoggerLevel.Info, XLoggerFormats.StandardLogFormat);
            }
        }

        public override void Log(string message, Exception? exception, FusionXLoggerLevel level, XLoggerFormats format)
        {
            if (_isConsoleLoggingEnabled)
            {
                WriteToConsole(message, exception, level, format);
            }
            else
            {
                WriteToFile(message, exception, level, format);
            }
        }

        public override void LogCritical(string message, Exception? exception, FusionXLoggerLevel level, XLoggerFormats format)
        {
            WriteToFile(message, exception, level, format);
        }

        public override void LogWarning(string message, Exception? exception, FusionXLoggerLevel level, XLoggerFormats format)
        {
            WriteToFile(message, exception, level, format);
        }

        public override void LogInfo(string message, Exception? exception, FusionXLoggerLevel level, XLoggerFormats format)
        {
            WriteToFile(message, exception, level, format);
        }

        public override void LogDebug(string message, Exception? exception, FusionXLoggerLevel level, XLoggerFormats format)
        {
            WriteToFile(message, exception, level, format);
        }

        public override void LogPerformance(string taskName, TimeSpan timeTaken, FusionXLoggerLevel level, XLoggerFormats format)
        {
            var message = $"Task '{taskName}' completed in {timeTaken.TotalMilliseconds} ms.";
            WriteToFile(message, null, level, format);
        }

        public override void LogWithTag(string tag, string message, FusionXLoggerLevel level, XLoggerFormats format)
        {
            var taggedMessage = $"[{tag}] {message}";
            WriteToFile(taggedMessage, null, level, format);
        }

        public override void LogError(string message, Exception exception, FusionXLoggerLevel level, XLoggerFormats format)
        {
            WriteToFile(message, exception, level, format);
        }

        public override void LogStructuredData(string message, Dictionary<string, string> data, FusionXLoggerLevel level)
        {
            _xFusionXMLWriter.Write(data);
        }

        public override void LogStructuredData<T>(string message, List<T> data, FusionXLoggerLevel level)
        {
            _xFusionXMLWriter.Write(data);
        }

        #endregion
    }
}
