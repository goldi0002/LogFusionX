using LogFusionX.Core.Configurations;
using LogFusionX.Core.Utils;
using LogFusionX.DBWriter;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;


namespace LogFusionX.Core.Loggers
{
    internal class XFileLogger : XLogger
    {
        private readonly string _XLoggerFilePath;
        private readonly string _XLoggerFileName;
        private readonly XFileLoggerConfigurationOptions? _ConfigurationOptions;
        public XFileLogger(string filePath, string fileName) : base(filePath, fileName)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentNullException(nameof(filePath));
            _XLoggerFilePath = filePath;
            _XLoggerFileName = fileName;
        }
        public XFileLogger(string filePath, string fileName, XFileLoggerConfigurationOptions xFileLoggerConfigurationOptions) : base(filePath, fileName, xFileLoggerConfigurationOptions.MaxFileSizeInMB)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentNullException(nameof(filePath));
            _XLoggerFilePath = filePath;
            _XLoggerFileName = fileName;
            _ConfigurationOptions = xFileLoggerConfigurationOptions;
        }
        public XFileLogger(XFileLoggerConfigurationOptions xFileLoggerConfigurationOptions) : base(xFileLoggerConfigurationOptions.LogDirectory, xFileLoggerConfigurationOptions.LogFileName, xFileLoggerConfigurationOptions.MaxFileSizeInMB, xFileLoggerConfigurationOptions)
        {
            if (string.IsNullOrEmpty(xFileLoggerConfigurationOptions.LogDirectory)) throw new ArgumentNullException(nameof(xFileLoggerConfigurationOptions.LogDirectory));
            _XLoggerFilePath = xFileLoggerConfigurationOptions.LogDirectory;
            _XLoggerFileName = xFileLoggerConfigurationOptions.LogFileName;
            _ConfigurationOptions = xFileLoggerConfigurationOptions;
        }
    }
}
