using System;

namespace LogFusionX.Core.Configurations
{
    public enum XLoggerFormats
    {
        /// <summary>
        /// Standard log format
        /// </summary>
        StandardLogFormat,

        /// <summary>
        /// This log format is obsolete and should not be used. Consider using another log format.
        /// </summary>
        [Obsolete("This log format is obsolete. Please use 'StandardLogFormat' or 'SimplLogFormat' instead.")]
        StandardErrLogFormat,

        /// <summary>
        /// Simple log format for line-by-line logs
        /// </summary>
        SimplLogFormat,
    }
    public enum XLoggerFolderFormat
    {
        StandardLogFolderFormat,
        SimplLogFolderFormat
    }
}
