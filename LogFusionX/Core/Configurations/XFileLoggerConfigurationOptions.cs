using LogFusionX.Core.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogFusionX.Core.Configurations
{
    public class XFileLoggerConfigurationOptions
    {
        /// <summary>
        /// Minimum logger level to record.
        /// </summary>
        public FusionXLoggerLevel LoggerLevel { get; set; } = FusionXLoggerLevel.None;
        /// <summary>
        /// The directory where log files will be stored.
        /// </summary>
        public string LogDirectory { get; set; } = "Logs";
        /// <summary>
        /// The name of the log file. Default is 'Log'.
        /// </summary>
        public string LogFileName { get; set; } = "Log";
        /// <summary>
        /// The file extension for the log file. Default is '.txt'.
        /// </summary>
        public string FileExtension { get; set; } = ".txt";
        /// <summary>
        /// Maximum file size (in MB) before rolling over to a new file.
        /// </summary>
        public int MaxFileSizeInMB { get; set; } = 10;
        /// <summary>
        /// Maximum number of log files to retain.
        /// </summary>
        public int MaxRetainedFiles { get; set; } = 5;
        /// <summary>
        /// Enable or disable thread-safe logging.
        /// </summary>
        public bool EnableThreadSafety { get; set; } = true;
        /// <summary>
        /// The log message format (e.g., "{Timestamp} [{Level}] {Message}").
        /// </summary>
        public string LogFormat { get; set; } = "{Timestamp} [{Level}] {Message}";

        /// <summary>
        /// Date format used in log files.
        /// </summary>
        public string DateFormat { get; set; } = "yyyy-MM-dd HH:mm:ss.fff";

        public XFileLoggerConfigurationOptions()
        {

        }
    }
}
