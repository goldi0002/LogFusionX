using System;
using System.Collections.Generic;
using System.Text;

namespace LogFusionX.Core.Utils
{
    public abstract class XFileLoggerBase
    {
        protected string _fileFullPath;
        protected XFileLoggerBase(string filePath)
        {
            _fileFullPath = filePath;
        }
        public abstract void Write(object data);
    }
}
