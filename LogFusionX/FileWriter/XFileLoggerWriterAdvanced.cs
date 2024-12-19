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

        private readonly BlockingCollection<string> _logQueue;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly Task _writerTask;

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
            _logQueue = new BlockingCollection<string>(boundedCapacity: 10_000);
            _cancellationTokenSource = new CancellationTokenSource();
            _writerTask = Task.Run(ProcessLogQueueAsync);
            InitializeWriter();
            _xLoggerFolderFormat = xLoggerFolderFormat;
        }
        public XFileLoggerWriterAdvanced(XFileLoggerConfigurationOptions xFileLoggerConfigurationOptions)
        {
            _logDirectory = xFileLoggerConfigurationOptions.LogDirectory;
            _baseFileName = xFileLoggerConfigurationOptions.LogFileName;
            _maxFileSizeInBytes = xFileLoggerConfigurationOptions.MaxFileSizeInMB * 1024 * 1024;
            _dateFormat = xFileLoggerConfigurationOptions.xLoggerFolderDateFormat;
            _xLoggerFolderFormat = xFileLoggerConfigurationOptions.xLoggerFolderFormat;
            Directory.CreateDirectory(_logDirectory);
            _logQueue = new BlockingCollection<string>(boundedCapacity: 10_000);
            _cancellationTokenSource = new CancellationTokenSource();
            _writerTask = Task.Run(ProcessLogQueueAsync);
            InitializeWriter();
        }
        private void InitializeWriter()
        {
            lock (_syncLock)
            {
                _streamWriter?.Dispose();
                _fileStream?.Dispose();
                if (_xLoggerFolderFormat == XLoggerFolderFormat.StandardLogFolderFormat)
                {
                    string dateFolder = DateTime.Now.ToString(_dateFormat);
                    string folderPath = Path.Combine(_logDirectory, dateFolder);
                    Directory.CreateDirectory(folderPath); //create folder for today if not exists
                    string filePath = GetNextLogFilePath(folderPath);
                    _fileStream = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite, 8192, useAsync: true);
                    _streamWriter = new StreamWriter(_fileStream, Encoding.UTF8) { AutoFlush = false };
                }
                else
                {
                    string filePath = GetNextLogFilePath(_logDirectory);
                    _fileStream = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite, 8192, useAsync: true);
                    _streamWriter = new StreamWriter(_fileStream, Encoding.UTF8) { AutoFlush = false };
                }
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
                _logQueue.Add(logMessage);
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
                    InitializeWriter(); // Rotate file
                }

                _streamWriter.WriteLine(logMessage);
                _streamWriter.Flush();
            }
        }

        private async Task ProcessLogQueueAsync()
        {
            var batch = new List<string>(100);
            try
            {
                while (!_cancellationTokenSource.Token.IsCancellationRequested || !_logQueue.IsCompleted)
                {
                    batch.Clear();

                    // Collect logs into a batch
                    while (batch.Count < 100 && _logQueue.TryTake(out string log, 100))
                    {
                        batch.Add(log);
                    }

                    if (batch.Count > 0)
                    {
                        await WriteLogsAsync(batch);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Graceful shutdown
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing log queue: {ex.Message}");
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

            _cancellationTokenSource.Cancel();
            _logQueue.CompleteAdding();

            try
            {
                _writerTask.Wait(); // Ensure background task completes
            }
            catch (AggregateException) { } // Ignore task exceptions on dispose

            lock (_syncLock)
            {
                _streamWriter?.Dispose();
                _fileStream?.Dispose();
            }

            _cancellationTokenSource.Dispose();
            _disposed = true;
        }
    }
}