using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LogFusionX.FileWriter
{
    public abstract class XFileLoggerHelper
    {
        protected bool IsDirectoryExist(string path)
        {
            return Directory.Exists(path);
        }
        protected bool IsFileExist(string path)
        {
            return File.Exists(path);
        }
    }
}
