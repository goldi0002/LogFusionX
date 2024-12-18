using LogFusionX.Core.Configurations;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogFusionX.Core.Interfaces
{
    public interface IFusionXLogWriter
    {
        bool WriteLog(XDbLogEntry xDbLogEntry);
    }
}
