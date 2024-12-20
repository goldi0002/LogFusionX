using LogFusionX.Core.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogFusionX.Core.Loggers
{
    internal class FusionXConsoleLogger
    {
        public FusionXConsoleLogger()
        {

        }
        private ConsoleColor GetLogLevelColor(FusionXLoggerLevel fusionXLoggerLevel)
        {
            return fusionXLoggerLevel switch
            {
                FusionXLoggerLevel.Trace => ConsoleColor.Gray,
                FusionXLoggerLevel.Debug => ConsoleColor.Cyan,
                FusionXLoggerLevel.Info => ConsoleColor.Green,
                FusionXLoggerLevel.Warn => ConsoleColor.Yellow,
                FusionXLoggerLevel.Error => ConsoleColor.Red,
                FusionXLoggerLevel.Critical => ConsoleColor.Magenta,
                _ => ConsoleColor.White,
            };
        }
        public void Log(string message, FusionXLoggerLevel level)
        {
            Console.ForegroundColor = GetLogLevelColor(level);
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
