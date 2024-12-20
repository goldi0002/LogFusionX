using System;
using System.Collections.Generic;
using System.Text;

namespace LogFusionX.StructuredLogWriter
{
    [Serializable]
    internal sealed class StructuredLogEntry
    {
        public string? message {  get; set; }
        public Dictionary<string,string>? logData { get; set; }
        public string? logLevel { get; set; }
    }
}
