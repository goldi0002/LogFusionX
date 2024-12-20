using LogFusionX.Core.Configurations;
using LogFusionX.Core.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogFusionX.Core.Loggers
{
    /// <summary>
    /// Base class for logging. Provides an abstract layer for common logging operations.
    /// </summary>
    public abstract class LoggerBase : LogStructureBase
    {
        /// <summary>
        /// Logs a simple message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public abstract void Log(string message);

        /// <summary>
        /// Logs a message with an optional exception and metadata.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="exception">The exception (if any).</param>
        /// <param name="fusionXLoggerLevel">The logging level (e.g., Info, Debug, Error).</param>
        /// <param name="xLogFormat">The format of the log entry.</param>
        /// <exception cref="ArgumentNullException">Thrown when the message is null or empty.</exception>
        public abstract void Log(string message, Exception? exception, FusionXLoggerLevel fusionXLoggerLevel, XLoggerFormats xLogFormat);

        /// <summary>
        /// Logs a critical error message with an optional exception.
        /// </summary>
        public abstract void LogCritical(string message, Exception? exception, FusionXLoggerLevel fusionXLoggerLevel, XLoggerFormats xLogFormat);

        /// <summary>
        /// Logs a warning message with an optional exception.
        /// </summary>
        public abstract void LogWarning(string message, Exception? exception, FusionXLoggerLevel fusionXLoggerLevel, XLoggerFormats xLogFormat);

        /// <summary>
        /// Logs an informational message with an optional exception.
        /// </summary>
        public abstract void LogInfo(string message, Exception? exception, FusionXLoggerLevel fusionXLoggerLevel, XLoggerFormats xLogFormat);

        /// <summary>
        /// Logs a debug message with an optional exception.
        /// </summary>
        public abstract void LogDebug(string message, Exception? exception, FusionXLoggerLevel fusionXLoggerLevel, XLoggerFormats xLogFormat);

        /// <summary>
        /// Logs performance-related information about a task.
        /// </summary>
        public abstract void LogPerformance(string taskName, TimeSpan timeTaken, FusionXLoggerLevel fusionXLoggerLevel, XLoggerFormats xLogFormat);

        /// <summary>
        /// Logs a message with a custom tag.
        /// </summary>
        public abstract void LogWithTag(string tag, string message, FusionXLoggerLevel fusionXLoggerLevel, XLoggerFormats xLogFormat);

        /// <summary>
        /// Logs an error message with a mandatory exception.
        /// </summary>
        public abstract void LogError(string message, Exception exception, FusionXLoggerLevel fusionXLoggerLevel, XLoggerFormats xLogFormat);
    }

    /// <summary>
    /// Base class for structured logging (e.g., key-value pairs or custom objects).
    /// </summary>
    public abstract class LogStructureBase
    {
        /// <summary>
        /// Validates input parameters to ensure they are not null or empty.
        /// Throws an exception if validation fails.
        /// </summary>
        /// <param name="param">The parameter to validate.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <exception cref="ArgumentNullException">Thrown when the parameter is null or empty.</exception>
        protected void ValidateInput(string param, string paramName)
        {
            if (string.IsNullOrWhiteSpace(param))
                throw new ArgumentNullException(paramName, "Input cannot be null, empty, or whitespace.");
        }
        /// <summary>
        /// Logs structured data as key-value pairs.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="data">The key-value pairs to log.</param>
        /// <param name="fusionXLoggerLevel">The logging level (e.g., Info, Debug, Error).</param>
        /// <exception cref="ArgumentNullException">Thrown when message or data is null.</exception>
        public abstract void LogStructuredData(string message, Dictionary<string, string> data, FusionXLoggerLevel fusionXLoggerLevel);
        /// <summary>
        /// Logs structured data as a list of custom objects.
        /// </summary>
        /// <typeparam name="T">The type of the custom objects.</typeparam>
        /// <param name="message">The message to log.</param>
        /// <param name="data">The list of custom objects to log.</param>
        /// <param name="fusionXLoggerLevel">The logging level (e.g., Info, Debug, Error).</param>
        /// <exception cref="ArgumentNullException">Thrown when message or data is null.</exception>
        public abstract void LogStructuredData<T>(string message, List<T> data, FusionXLoggerLevel fusionXLoggerLevel);
    }
}
