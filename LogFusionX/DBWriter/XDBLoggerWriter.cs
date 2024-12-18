using LogFusionX.Core.Configurations;
using LogFusionX.Core.Interfaces;
using LogFusionX.Core.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogFusionX.DBWriter
{
    internal class XDBLoggerWriter : IFusionXLogWriter
    {
        private readonly XsqlServer xsqlServer;
        public XDBLoggerWriter(string connectionString)
        {
            xsqlServer = new XsqlServer(connectionString, "FUSION_X_LOG");
        }
        public XDBLoggerWriter(string connectionString, string tableName)
        {
            xsqlServer = new XsqlServer(connectionString, tableName);
        }

        public bool WriteLog(XDbLogEntry xDbLogEntry)
        {
            return xsqlServer.InsertLog(xDbLogEntry);
        }
    }
}
