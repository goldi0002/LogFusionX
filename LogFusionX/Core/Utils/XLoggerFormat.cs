using LogFusionX.Core.Configurations;
using System;
using System.Linq;
using System.Text;
using System.Threading;

namespace LogFusionX.Core.Utils
{
    public sealed class XLoggerFormat : XLoggerFormatHelper
    {
        private readonly string _logTimeStampFormat;

        public XLoggerFormat(string? logTimeStampFormat = "yyyy-MM-dd HH:mm:ss.fff")
        {
            _logTimeStampFormat = !string.IsNullOrEmpty(logTimeStampFormat) ? logTimeStampFormat : "yyyy-MM-dd HH:mm:ss.fff";
        }

        public string GetLogFormat(FusionXLoggerLevel xLoggerLevel, string message, Exception? exception, string? methodName, XLoggerFormats xLoggerFormats)
        {
            return xLoggerFormats switch
            {
                XLoggerFormats.SimplLogFormat => GetMinimalistLog(message, GetTimestamp(), xLoggerLevel, methodName),

                XLoggerFormats.StandardLogFormat => xLoggerLevel switch
                {
                    FusionXLoggerLevel.None => GetInfoLog(message, GetTimestamp(), FusionXLoggerLevel.Info, methodName),
                    FusionXLoggerLevel.Error => GetErrorLog(message, GetTimestamp(), xLoggerLevel, methodName, FormatException(exception)),
                    FusionXLoggerLevel.Info => GetInfoLog(message, GetTimestamp(), FusionXLoggerLevel.Info, methodName),
                    FusionXLoggerLevel.Warn => GetWarningLog(message, GetTimestamp(), xLoggerLevel, methodName),
                    FusionXLoggerLevel.Fatal => GetFatalLog(message, GetTimestamp(), xLoggerLevel, methodName, FormatException(exception)),
                    FusionXLoggerLevel.Debug => GetDebugLog(message, GetTimestamp(), xLoggerLevel, methodName),
                    FusionXLoggerLevel.Critical => GetCriticalLog(message, GetTimestamp(), xLoggerLevel, methodName, FormatException(exception)),
                    _ => GetAnsiColoredLog(message, GetTimestamp(), xLoggerLevel, methodName)
                },

                _ => GetMinimalistLog(message, GetTimestamp(), xLoggerLevel, methodName)
            };
        }

        private string GetTimestamp() => DateTime.Now.ToString(_logTimeStampFormat);
    }

    public abstract class XLoggerFormatHelper
    {
        protected XLoggerFormatHelper() { }

        private static string GenerateLine(char character, int length) => new string(character, length);
        private static string Get95Lines() => GenerateLine('-', 95);
        private static string Get145Lines() => GenerateLine('=', 145);

        private static StringBuilder BuildCommonLog(StringBuilder builder, string timestamp, FusionXLoggerLevel level, string? methodName, string message)
        {
            builder.AppendLine(Get145Lines())
                .AppendLine($"🔹 {level.ToString().ToUpper()} LOG ENTRY 🔹")
                .AppendLine($"🕒 Timestamp: {timestamp}")
                .AppendLine($"🔰 Log Level: {level}")
                .AppendLine($"📍 Method: {methodName ?? "N/A"}")
                .AppendLine(Get95Lines())
                .AppendLine($"Log Message:")
                .AppendLine(message)
                .AppendLine(Get95Lines());

            return builder;
        }

        protected static string GetErrorLog(string message, string timestamp, FusionXLoggerLevel level, string? methodName, string exceptionDetails)
        {
            var builder = new StringBuilder();
            BuildCommonLog(builder, timestamp, level, methodName, message)
                .AppendLine("Exception Details:")
                .AppendLine(exceptionDetails)
                .AppendLine(Get145Lines());

            return builder.ToString();
        }

        protected static string GetInfoLog(string message, string timestamp, FusionXLoggerLevel level, string? methodName)
        {
            var builder = new StringBuilder();
            BuildCommonLog(builder, timestamp, level, methodName, message)
                .AppendLine($"🔵 End of {level} Log Entry")
                .AppendLine(Get145Lines());

            return builder.ToString();
        }

        protected static string GetWarningLog(string message, string timestamp, FusionXLoggerLevel level, string? methodName)
        {
            var builder = new StringBuilder();
            BuildCommonLog(builder, timestamp, level, methodName, message)
                .AppendLine($"⚠️ End of {level} Log Entry")
                .AppendLine(Get145Lines());

            return builder.ToString();
        }

        protected static string GetDebugLog(string message, string timestamp, FusionXLoggerLevel level, string? methodName)
        {
            var builder = new StringBuilder();
            BuildCommonLog(builder, timestamp, level, methodName, message)
                .AppendLine($"🟢 End of {level} Log Entry")
                .AppendLine(Get145Lines());

            return builder.ToString();
        }

        protected static string GetFatalLog(string message, string timestamp, FusionXLoggerLevel level, string? methodName, string exceptionDetails)
        {
            var builder = new StringBuilder();
            BuildCommonLog(builder, timestamp, level, methodName, message)
                .AppendLine("Exception Details:")
                .AppendLine(exceptionDetails)
                .AppendLine(Get145Lines());

            return builder.ToString();
        }

        protected static string FormatException(Exception? exception)
        {
            if (exception == null) return "No exception details available.";

            var builder = new StringBuilder();
            builder.AppendLine($"Exception Type: {exception.GetType()}")
                .AppendLine($"Message: {exception.Message}")
                .AppendLine($"Stack Trace: {exception.StackTrace}");

            var inner = exception.InnerException;
            while (inner != null)
            {
                builder.AppendLine(Get95Lines())
                    .AppendLine($"Inner Exception Type: {inner.GetType()}")
                    .AppendLine($"Inner Message: {inner.Message}")
                    .AppendLine($"Inner Stack Trace: {inner.StackTrace}");
                inner = inner.InnerException;
            }

            return builder.ToString();
        }

        protected static string GetMinimalistLog(string message, string timestamp, FusionXLoggerLevel level, string? methodName)
        {
            return $"[{timestamp}] [{level}] - Method: {methodName ?? "N/A"} - Message: {message}";
        }

        protected static string GetAnsiColoredLog(string message, string timestamp, FusionXLoggerLevel level, string? methodName)
        {
            var colorCode = level switch
            {
                FusionXLoggerLevel.Error => "\u001b[31m",
                FusionXLoggerLevel.Warn => "\u001b[33m",
                FusionXLoggerLevel.Info => "\u001b[32m",
                FusionXLoggerLevel.Debug => "\u001b[36m",
                _ => "\u001b[0m"
            };

            return $"{colorCode}[{timestamp}] [{level}] - Method: {methodName ?? "N/A"} - Message: {message}\u001b[0m";
        }

        protected static string GetCriticalLog(string message, string timestamp, FusionXLoggerLevel level, string? methodName, string exceptionDetails)
        {
            var builder = new StringBuilder();
            BuildCommonLog(builder, timestamp, level, methodName, message)
                .AppendLine("Critical Exception Details:")
                .AppendLine(exceptionDetails)
                .AppendLine(Get145Lines());

            return builder.ToString();
        }
    }
}
