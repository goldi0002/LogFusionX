using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LogFusionX.FileWriter
{
    internal class XFileLoggerWriter : XFileLoggerHelper, IDisposable
    {
        private readonly string _xFileLoggerWriterFilePath;
        private readonly StreamWriter _xStreamWriter;
        private bool _disposed = false;
        public XFileLoggerWriter(string xFileLoggerWriterFilePath)
        {
            if (string.IsNullOrEmpty(xFileLoggerWriterFilePath)) throw new ArgumentNullException(nameof(xFileLoggerWriterFilePath));
            _xFileLoggerWriterFilePath = xFileLoggerWriterFilePath;
            _xStreamWriter = new StreamWriter(new FileStream(_xFileLoggerWriterFilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite, bufferSize: 8192, useAsync: true))
            {
                AutoFlush = false
            };
        }
        public void WriteLogToFile(string Input)
        {
            try
            {
                _xStreamWriter.WriteLine(Input);
            }
            catch (IOException ex)
            {
                throw new Exception(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task WriteLogToFileAsync(string Input)
        {
            try
            {
                await _xStreamWriter.WriteLineAsync(Input);
            }
            catch (IOException ex)
            {
                throw new Exception(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public void Flush()
        {
            _xStreamWriter.Flush();
        }
        public void Close()
        {
            _xStreamWriter.Close();
        }
        public async Task FlushAsync()
        {
            await _xStreamWriter.FlushAsync();
        }
        public void Dispose()
        {
            if (!_disposed)
            {
                _xStreamWriter.Dispose();
                _disposed = true;
            }
        }
    }
}
