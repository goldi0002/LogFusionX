using LogFusionX.Core.Configurations;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LogFusionX.FileWriter
{
    internal class XFileLoggerWriterAdvanced : XFileLoggerHelper, IDisposable
    {
        private readonly string _logDirectory;
        private readonly string _baseFileName;
        private readonly int _maxFileSizeInBytes;
        private readonly string _dateFormat = "yyyy-MM-dd";
        private int _fileIndex = 0;
        private bool _disposed = false;

        private readonly Queue<string> _logQueue;
        private readonly object _logQueueLock = new object();

        private StreamWriter _streamWriter = null!;
        private FileStream _fileStream = null!;

        private readonly object _syncLock = new object();
        private readonly XLoggerFolderFormat _xLoggerFolderFormat = XLoggerFolderFormat.SimplLogFolderFormat;

        public XFileLoggerWriterAdvanced(string logDirectory, string baseFileName, int maxFileSizeInMB = 10, XLoggerFolderFormat xLoggerFolderFormat = default)
        {
            if (string.IsNullOrWhiteSpace(logDirectory)) throw new ArgumentNullException(nameof(logDirectory));
            if (string.IsNullOrWhiteSpace(baseFileName)) throw new ArgumentNullException(nameof(baseFileName));
            _logDirectory = logDirectory;
            _baseFileName = baseFileName;
            _maxFileSizeInBytes = maxFileSizeInMB * 1024 * 1024;
            Directory.CreateDirectory(_logDirectory);
            _logQueue = new Queue<string>();
            InitializeWriter();
            _xLoggerFolderFormat = xLoggerFolderFormat;
        }
        public XFileLoggerWriterAdvanced(XLoggerConfigurationOptions XLoggerConfigurationOptions)
        {
            _logDirectory = XLoggerConfigurationOptions.LogDirectory;
            _baseFileName = XLoggerConfigurationOptions.LogFileName;
            _maxFileSizeInBytes = XLoggerConfigurationOptions.MaxFileSizeInMB * 1024 * 1024;
            _dateFormat = XLoggerConfigurationOptions.xLoggerFolderDateFormat;
            _xLoggerFolderFormat = XLoggerConfigurationOptions.xLoggerFolderFormat;
            Directory.CreateDirectory(_logDirectory);
            _logQueue = new Queue<string>();
            InitializeWriter();
        }
        private void InitializeWriter()
        {
            lock (_syncLock)
            {
                _streamWriter?.Dispose();
                _fileStream?.Dispose();

                string folderPath = _xLoggerFolderFormat == XLoggerFolderFormat.StandardLogFolderFormat
                    ? Path.Combine(_logDirectory, DateTime.Now.ToString(_dateFormat))
                    : _logDirectory;

                Directory.CreateDirectory(folderPath);
                string filePath = GetNextLogFilePath(folderPath);
                _fileStream = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite, 8192);
                _streamWriter = new StreamWriter(_fileStream, Encoding.UTF8) { AutoFlush = false };
            }
        }
        private string GetNextLogFilePath(string folderPath)
        {
            _fileIndex++;
            return Path.Combine(folderPath, $"{_baseFileName}_{_fileIndex:00000}.log");
        }

        /// <summary>
        /// Asynchronously enqueue a log message.
        /// </summary>
        public void EnqueueLog(string logMessage)
        {
            if (!string.IsNullOrWhiteSpace(logMessage))
            {
                lock (_logQueueLock)
                {
                    _logQueue.Enqueue(logMessage);
                }

                ProcessLogQueue();
            }
        }
        private void ProcessLogQueue()
        {
            lock (_logQueueLock)
            {
                while (_logQueue.Count > 0)
                {
                    string logMessage = _logQueue.Dequeue();
                    WriteLog(logMessage);
                }
            }
        }
        /// <summary>
        /// Synchronously write a log message.
        /// </summary>
        public void WriteLog(string logMessage)
        {
            if (string.IsNullOrWhiteSpace(logMessage)) return;

            lock (_syncLock)
            {
                if (_fileStream.Length >= _maxFileSizeInBytes)
                {
                    InitializeWriter(); // Rotate file by creating a new one
                }

                _streamWriter.WriteLine(logMessage);
                _streamWriter.Flush();
            }
        }

        private async Task WriteLogsAsync(List<string> logs)
        {
            lock (_syncLock)
            {
                if (_fileStream.Length >= _maxFileSizeInBytes)
                {
                    InitializeWriter(); // Rotate file
                }
            }

            foreach (string log in logs)
            {
                await _streamWriter.WriteLineAsync(log);
            }

            await _streamWriter.FlushAsync();
        }

        /// <summary>
        /// Flushes the current stream.
        /// </summary>
        public void Flush()
        {
            lock (_syncLock)
            {
                _streamWriter?.Flush();
            }
        }

        /// <summary>
        /// Clean up and dispose resources.
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;

            lock (_syncLock)
            {
                _streamWriter?.Dispose();
                _fileStream?.Dispose();
            }

            _disposed = true;
        }
    }
}