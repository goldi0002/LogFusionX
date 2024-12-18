using System;
using System.Collections.Generic;
using System.Text;

namespace LogFusionX.Core.Interfaces
{
    public interface IFusionXLogger
    {
        void Log(string message);
        void Log(string message, Exception exception);
        void Log(Exception exception);
    }
}
