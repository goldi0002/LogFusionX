using LogFusionX.Core.Configurations;
using LogFusionX.Core.Utils;
using LogFusionX.DBWriter;
using System;
using System.IO;

namespace LogFusionX.Core.Loggers
{
    internal class XFileLogger : XLogger
    {
        private readonly string _XLoggerFilePath;
        private readonly string _XLoggerFileName;
        private readonly XLoggerConfigurationOptions? _ConfigurationOptions;

        // Optimize constructor overloads to avoid redundant validation
        public XFileLogger(string filePath, string fileName) : this(filePath, fileName, null)
        {
        }

        public XFileLogger(string filePath, string fileName, XLoggerConfigurationOptions? XLoggerConfigurationOptions) : base(filePath, fileName, XLoggerConfigurationOptions?.MaxFileSizeInMB ?? 0)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentNullException(nameof(filePath));

            _XLoggerFilePath = filePath;
            _XLoggerFileName = fileName;
            _ConfigurationOptions = XLoggerConfigurationOptions;
            ValidateLogDirectory(_XLoggerFilePath);
        }

        public XFileLogger(XLoggerConfigurationOptions XLoggerConfigurationOptions) : base(XLoggerConfigurationOptions)
        {
            if (XLoggerConfigurationOptions == null || string.IsNullOrEmpty(XLoggerConfigurationOptions.LogDirectory))
                throw new ArgumentNullException(nameof(XLoggerConfigurationOptions.LogDirectory));

            _XLoggerFilePath = XLoggerConfigurationOptions.LogDirectory;
            _XLoggerFileName = XLoggerConfigurationOptions.LogFileName;
            _ConfigurationOptions = XLoggerConfigurationOptions;
            ValidateLogDirectory(_XLoggerFilePath);
        }

        // Validate the log directory exists
        private void ValidateLogDirectory(string logDirectory)
        {
            if (!Directory.Exists(logDirectory))
            {
                try
                {
                    Directory.CreateDirectory(logDirectory);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to create directory {logDirectory}", ex);
                }
            }
        }
    }
}