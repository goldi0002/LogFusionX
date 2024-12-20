using LogFusionX.Core.Configurations;
using LogFusionX.Core.Utils;
using LogFusionX.DBWriter;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace LogFusionX.Core.Loggers
{
    /// <summary>
    /// FusionXLogger provides file and database logging capabilities with extensible options.
    /// </summary>
    public class FusionXLogger
    {
        private readonly bool _isDbLoggingEnabled;
        private readonly bool _isFileLoggingEnabled;

        private readonly XFileLogger? _fileLogger;
        private readonly XDBLoggerWriter? _dbLoggerWriter;
        private readonly XLoggerHelper _loggerHelper;
        private readonly Configurations.XLoggerFormats xLoggerLoggingFormat = XLoggerFormats.SimplLogFormat;

        /// <summary>
        /// Optional custom log handler for extending logging functionality.
        /// </summary>
        private readonly Action<string, Exception?>? _customLogHandler;

        #region Constructors

        /// <summary>
        /// Constructor for file-based logging.
        /// </summary>
        public FusionXLogger(string filePath, string fileName)
        {
            _isFileLoggingEnabled = true;
            _fileLogger = new XFileLogger(GetFileLoggerConfigurationOptions(filePath, fileName));
            _loggerHelper = new XLoggerHelper();
        }

        /// <summary>
        /// Constructor for database-based logging.
        /// </summary>
        public FusionXLogger(bool isDatabaseLogging, string sqlServerConnectionString, string tableName = "FusionXLog")
        {
            if (!isDatabaseLogging)
                throw new ArgumentException("This constructor is intended for database logging only.");
            _isDbLoggingEnabled = true;
            _dbLoggerWriter = new XDBLoggerWriter(sqlServerConnectionString, tableName);
            _loggerHelper = new XLoggerHelper();
        }

        /// <summary>
        /// Constructor for custom logging with an optional delegate.
        /// </summary>
        public FusionXLogger(Action<string, Exception?> customLogHandler)
        {
            _customLogHandler = customLogHandler;
            _loggerHelper = new XLoggerHelper();
        }

        /// <summary>
        /// Constructor supporting both file and database logging.
        /// </summary>
        public FusionXLogger(string filePath, string fileName, string sqlServerConnectionString, string tableName = "FusionXLog")
        {
            _isFileLoggingEnabled = true;
            _isDbLoggingEnabled = true;
            _fileLogger = new XFileLogger(GetFileLoggerConfigurationOptions(filePath, fileName));
            _dbLoggerWriter = new XDBLoggerWriter(sqlServerConnectionString, tableName);
            _loggerHelper = new XLoggerHelper();
        }
        public FusionXLogger(XFileLoggerConfigurationOptions xFileLoggerConfigurationOptions)
        {
            _isFileLoggingEnabled = true;
            _fileLogger = new XFileLogger(xFileLoggerConfigurationOptions);
            _loggerHelper = new XLoggerHelper();
            xLoggerLoggingFormat = xFileLoggerConfigurationOptions.xLoggerFormat;
        }
        #endregion

        #region Public Logging Methods
        private XFileLoggerConfigurationOptions GetFileLoggerConfigurationOptions(string filePath, string fileName)
        {
            return new XFileLoggerConfigurationOptions()
            {
                LogFileName = fileName,
                LogDirectory = filePath,
                xLoggerFormat = Configurations.XLoggerFormats.SimplLogFormat
            };
        }
        /// <summary>
        /// Logs a message.
        /// </summary>
        public void Log(string message)
        {
            WriteLog(message, null, FusionXLoggerLevel.None);
        }

        /// <summary>
        /// Logs a message with an exception.
        /// </summary>
        public void Log(string message, Exception exception)
        {
            WriteLog(message, exception, FusionXLoggerLevel.None);
        }
        /// <summary>
        /// Logs an error message with an exception.
        /// </summary>
        public void LogError(string message, Exception exception)
        {
            WriteLog(message, exception, FusionXLoggerLevel.Error);
        }
        public void LogCritical(string message, Exception exception)
        {
            WriteLog(message, exception, FusionXLoggerLevel.Critical);
        }
        public void LogWarning(string message, Exception exception)
        {
            WriteLog(message, exception, FusionXLoggerLevel.Warn);
        }
        #endregion

        #region Private Helper Methods

        /// <summary>
        /// Writes the log to the appropriate destination(s).
        /// </summary>
        private void WriteLog(string message, Exception? exception, FusionXLoggerLevel level = FusionXLoggerLevel.Info)
        {
            if (_isFileLoggingEnabled)
            {
                _fileLogger?.Log(message, exception, level, xLoggerLoggingFormat);
            }

            if (_isDbLoggingEnabled)
            {
                WriteDbLog(message, exception, level);
            }
            _customLogHandler?.Invoke(message, exception);
        }

        /// <summary>
        /// Writes the log to the database.
        /// </summary>
        private void WriteDbLog(string message, Exception? exception, FusionXLoggerLevel level)
        {
            _dbLoggerWriter?.WriteLog(new XDbLogEntry().CreateLogEntry(
                0,
                log_level: level.ToString().ToUpper(),
                log_severity: 1,
                DateTime.Now,
                log_message: message,
                exception_message: exception?.Message,
                exception_type: exception?.GetType().ToString(),
                exception_stack_trace: exception?.StackTrace,
                thread_id: Thread.CurrentThread.ManagedThreadId.ToString(),
                application_name: null,
                environment_name: Environment.MachineName,
                framework_version: System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription,
                memory_usage_kb: Process.GetCurrentProcess().PrivateMemorySize64 / 1024 / 1024,
                client_ip: _loggerHelper.GetServerIpAddress()
            ));
        }

        #endregion
    }
}
