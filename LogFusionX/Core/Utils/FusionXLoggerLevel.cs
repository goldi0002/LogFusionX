using System;
using System.Collections.Generic;
using System.Text;

namespace LogFusionX.Core.Utils
{
    public enum FusionXLoggerLevel
    {
        /// <summary>
        /// No logging.
        /// </summary>
        None = 0,

        /// <summary>
        /// Trace level: very detailed logs, often used for debugging.
        /// </summary>
        Trace = 1,

        /// <summary>
        /// Debug level: detailed information used during development.
        /// </summary>
        Debug = 2,

        /// <summary>
        /// Information level: general application events.
        /// </summary>
        Info = 3,

        /// <summary>
        /// Warning level: unexpected events that are not yet errors.
        /// </summary>
        Warn = 4,

        /// <summary>
        /// Error level: application errors that need attention.
        /// </summary>
        Error = 5,

        /// <summary>
        /// Critical level: serious failures or crashes.
        /// </summary>
        Fatal = 6,

        /// <summary>
        /// Security level: logs related to security events, e.g., authentication failures.
        /// </summary>
        Security = 7,

        /// <summary>
        /// Performance level: logs related to performance monitoring.
        /// </summary>
        Performance = 8,

        /// <summary>
        /// Audit level: logs for auditing activities, e.g., access and changes.
        /// </summary>
        Audit = 9,

        /// <summary>
        /// Custom level: for user-defined log types.
        /// </summary>
        Custom = 10,

        Critical = 11
    }
}
