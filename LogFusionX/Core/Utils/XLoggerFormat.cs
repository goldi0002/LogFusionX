using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace LogFusionX.Core.Utils
{
    public sealed class XLoggerFormat : XLoggerFormatHelper
    {
        private readonly string _logTimeStampFormat = "yyyy-MM-dd HH:mm:ss.fff";
        public XLoggerFormat(string? logTimeStampFormat)
        {
            if (!string.IsNullOrEmpty(logTimeStampFormat))
            {
                _logTimeStampFormat = logTimeStampFormat;
            }
        }
        public string GetLogFormat(FusionXLoggerLevel xLoggerLevel, string message, Exception? exception, string? MethodName)
        {
            if (xLoggerLevel == FusionXLoggerLevel.Error)
            {
                return GetErrorLog(message, GetDateTimeWithFormat(), xLoggerLevel, MethodName, FormatException(exception));
            }
            else if (xLoggerLevel == FusionXLoggerLevel.Info || xLoggerLevel == FusionXLoggerLevel.None)
            {
                xLoggerLevel = FusionXLoggerLevel.Info;
                return GetInfoLog(message, GetDateTimeWithFormat(), xLoggerLevel, MethodName);
            }
            else if (xLoggerLevel == FusionXLoggerLevel.Warn)
            {
                return GetWarningLog(message, GetDateTimeWithFormat(), xLoggerLevel, MethodName);
            }
            else if (xLoggerLevel == FusionXLoggerLevel.Fatal)
            {
                return GetFatalLog(message, GetDateTimeWithFormat(), xLoggerLevel, MethodName, FormatException(exception));
            }
            else if (xLoggerLevel == FusionXLoggerLevel.Debug)
            {
                return GetDebugLog(message, GetDateTimeWithFormat(), xLoggerLevel, MethodName);
            }
            else if (xLoggerLevel == FusionXLoggerLevel.Critical)
            {
                return string.Empty;
            }
            else
            {
                return string.Empty;
            }
        }
        private string GetDateTimeWithFormat()
        {
            return DateTime.Now.ToString(_logTimeStampFormat);
        }

    }
    public abstract class XLoggerFormatHelper
    {
        protected XLoggerFormatHelper() { }

        // Generate a line of specified character and length
        private static string GenerateLine(char character, int length) => new string(character, length);

        // Line constants
        private static string Get95Lines() => GenerateLine('-', 95);
        private static string Get145LinesHeadFoot() => GenerateLine('=', 145);

        // Header/Footer for log entries
        private static string GetLogHeaderFooter(FusionXLoggerLevel xLoggerLevel) =>
          $"🔹 {xLoggerLevel.ToString().ToUpper()} LOG ENTRY {string.Concat(Enumerable.Repeat("🔹", 10))}";

        // General method to append lines with separators
        private static void AppendLineWithSeparator(StringBuilder builder, string content, char separator = '-')
        {
            builder.AppendLine(content);
            builder.AppendLine(GenerateLine(separator, content.Length));
        }

        /// <summary>
        /// Formats a generic log entry.
        /// </summary>
        private static StringBuilder BuildCommonLog(
            StringBuilder stringBuilder,
            string timestamp,
            FusionXLoggerLevel xLoggerLevel,
            string? methodName,
            string message)
        {
            stringBuilder.AppendLine(Get145LinesHeadFoot())
                .AppendLine(GetLogHeaderFooter(xLoggerLevel))
                .AppendLine()
                .AppendLine($"🕒 **Timestamp:** {timestamp}")
                .AppendLine($"🔰 **Log Level:** {xLoggerLevel.ToString().ToUpper()}")
                .AppendLine($"🔍 **Thread ID:** {Thread.CurrentThread.ManagedThreadId}")
                .AppendLine($"📍 **Method:** {methodName ?? "N/A"}")
                .AppendLine()
                .AppendLine(Get95Lines())
                .AppendLine($"**Log Message:**")
                .AppendLine(message)
                .AppendLine(Get95Lines());
            return stringBuilder;
        }

        /// <summary>
        /// Formats an error log entry.
        /// </summary>
        protected static string GetErrorLog(
            string message,
            string timestamp,
            FusionXLoggerLevel xLoggerLevel,
            string? methodName,
            string exceptionDetails)
        {
            var stringBuilder = new StringBuilder();
            BuildCommonLog(stringBuilder, timestamp, xLoggerLevel, methodName, message)
                .AppendLine("**Exception Details:**")
                .AppendLine(exceptionDetails)
                .AppendLine(Get95Lines())
                .AppendLine($"🔴 End of {xLoggerLevel} Log Entry")
                .AppendLine(Get145LinesHeadFoot())
                .AppendLine();
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Formats an info log entry.
        /// </summary>
        protected static string GetInfoLog(string message, string timestamp, FusionXLoggerLevel xLoggerLevel, string? methodName)
        {
            var stringBuilder = new StringBuilder();
            BuildCommonLog(stringBuilder, timestamp, xLoggerLevel, methodName, message)
                .AppendLine($"🔵 End of {xLoggerLevel} Log Entry")
                .AppendLine(Get145LinesHeadFoot())
                .AppendLine();
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Formats a warning log entry.
        /// </summary>
        protected static string GetWarningLog(string message, string timestamp, FusionXLoggerLevel xLoggerLevel, string? methodName)
        {
            var stringBuilder = new StringBuilder();
            BuildCommonLog(stringBuilder, timestamp, xLoggerLevel, methodName, message)
                .AppendLine($"⚠️ End of {xLoggerLevel} Log Entry")
                .AppendLine(Get145LinesHeadFoot())
                .AppendLine();
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Formats a debug log entry.
        /// </summary>
        protected static string GetDebugLog(string message, string timestamp, FusionXLoggerLevel xLoggerLevel, string? methodName)
        {
            var stringBuilder = new StringBuilder();
            BuildCommonLog(stringBuilder, timestamp, xLoggerLevel, methodName, message)
                .AppendLine($"🟢 End of {xLoggerLevel} Log Entry")
                .AppendLine(Get145LinesHeadFoot())
                .AppendLine();
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Formats a fatal log entry.
        /// </summary>
        protected static string GetFatalLog(
            string message,
            string timestamp,
            FusionXLoggerLevel xLoggerLevel,
            string? methodName,
            string exceptionDetails)
        {
            var stringBuilder = new StringBuilder();
            BuildCommonLog(stringBuilder, timestamp, xLoggerLevel, methodName, message)
                .AppendLine("**Exception Details:**")
                .AppendLine(exceptionDetails)
                .AppendLine(Get95Lines())
                .AppendLine($"💀 End of {xLoggerLevel} Log Entry")
                .AppendLine(Get145LinesHeadFoot())
                .AppendLine();
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Formats an exception with nested inner exception details.
        /// </summary>
        protected static string FormatException(Exception? exception)
        {
            var stringBuilder = new StringBuilder();
            if (exception != null)
            {
                stringBuilder.AppendLine($"⚡ **Exception Type:** {exception.GetType()}")
                    .AppendLine($"⚡ **Message:** {exception.Message}")
                    .AppendLine($"⚡ **Stack Trace:** {exception.StackTrace}");

                var innerException = exception.InnerException;
                while (innerException != null)
                {
                    stringBuilder.AppendLine(Get95Lines())
                        .AppendLine($"⚡ **Inner Exception Type:** {innerException.GetType()}")
                        .AppendLine($"⚡ **Inner Message:** {innerException.Message}")
                        .AppendLine($"⚡ **Inner Stack Trace:** {innerException.StackTrace}");
                    innerException = innerException.InnerException;
                }
            }
            else
            {
                stringBuilder.AppendLine("No exception details available.");
            }
            return stringBuilder.ToString();
        }
        protected static string GetMinimalistLog(string message, string timestamp, FusionXLoggerLevel xLoggerLevel, string? methodName, string threadId)
        {
            return $@"[{timestamp}] [{xLoggerLevel.ToString().ToUpper()}] - Thread: {threadId} - Method: {methodName ?? "N/A"}
            Message: {message}";
        }
        private static string GetHtmlLog(string message, string timestamp, FusionXLoggerLevel xLoggerLevel, string? methodName, string threadId, string exceptionDetails = "")
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("<html>")
                .AppendLine("<body style='font-family: Arial, sans-serif;'>")
                .AppendLine("<div style='border: 2px solid #4CAF50; padding: 10px; margin: 10px;'>")
                .AppendLine($"<h2 style='color: #4CAF50;'>[{xLoggerLevel.ToString().ToUpper()}] Log Entry</h2>")
                .AppendLine($"<p><strong>Timestamp:</strong> {timestamp}</p>")
                .AppendLine($"<p><strong>Thread:</strong> {threadId}</p>")
                .AppendLine($"<p><strong>Method:</strong> {methodName ?? "N/A"}</p>")
                .AppendLine("<hr>")
                .AppendLine($"<p><strong>Message:</strong> {message}</p>");

            if (!string.IsNullOrEmpty(exceptionDetails))
            {
                stringBuilder.AppendLine("<hr>")
                    .AppendLine($"<p><strong>Exception Details:</strong></p>")
                    .AppendLine($"<pre>{exceptionDetails}</pre>");
            }

            stringBuilder.AppendLine("</div>")
                .AppendLine("</body>")
                .AppendLine("</html>");
            return stringBuilder.ToString();
        }
        protected static string GetAnsiColoredLog(string message, string timestamp, FusionXLoggerLevel xLoggerLevel, string? methodName, string threadId)
        {
            string colorCode = xLoggerLevel switch
            {
                FusionXLoggerLevel.Error => "\u001b[31m", // Red
                FusionXLoggerLevel.Warn => "\u001b[33m", // Yellow
                FusionXLoggerLevel.Info => "\u001b[32m", // Green
                FusionXLoggerLevel.Debug => "\u001b[36m", // Cyan
                _ => "\u001b[0m" // Reset
            };

            return $"{colorCode}[{timestamp}] [{xLoggerLevel.ToString().ToUpper()}] - Thread: {threadId} - Method: {methodName ?? "N/A"}\nMessage: {message}\u001b[0m";
        }
        protected static string GetEnhancedLog(string message, string timestamp, FusionXLoggerLevel xLoggerLevel, string? methodName, string threadId, string exceptionDetails = "")
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(new string('═', 100)) // Top border
                .AppendLine($"🕒 Timestamp:      {timestamp}")
                .AppendLine($"⚡ Log Level:     {xLoggerLevel.ToString().ToUpper()}")
                .AppendLine($"📄 Method:        {methodName ?? "N/A"}")
                .AppendLine($"🔧 Thread:        {threadId}")
                .AppendLine(new string('-', 100)) // Divider
                .AppendLine($"✍️ Message:       {message}")
                .AppendLine(new string('-', 100)); // Divider

            if (!string.IsNullOrEmpty(exceptionDetails))
            {
                stringBuilder.AppendLine("🚨 Exception Details:")
                    .AppendLine(exceptionDetails)
                    .AppendLine(new string('-', 100)); // Divider
            }

            stringBuilder.AppendLine($"End of {xLoggerLevel} Log Entry").AppendLine(new string('═', 100)); // Bottom border
            return stringBuilder.ToString();
        }
    }
    internal enum XLoggerFormats
    {
        STND_LOG_FORMAT,
        STND_ERR_LOG_FORMAT,
        SIMPL_LOG_FORMAT,
        ADVNC_LOG_FORMAT
    }
}
