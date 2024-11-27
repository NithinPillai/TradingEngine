using System;
using System.Threading.Tasks.Dataflow;
using System.Threading;
using System.IO;

using Microsoft.Extensions.Options;

using TradingEngineServer.Logging.LoggingConfiguration;
using System.Threading.Tasks;

namespace TradingEngineServer.Logging
{
	public class TextLogger : AbstractLogger, ITextLogger
    {
        private readonly LoggerConfiguration _loggerConfiguration;

        public TextLogger(IOptions<LoggerConfiguration> loggingConfiguration) : base()
        {
            _loggerConfiguration = loggingConfiguration.Value ?? throw new ArgumentNullException(nameof(loggingConfiguration));

            if (_loggerConfiguration.LoggerType != LoggerType.Text)
            {
                throw new InvalidOperationException($"{nameof(TextLogger)} doesn't match LoggerType of {_loggerConfiguration.LoggerType}");
            }

            var now = DateTime.Now;
            // used for creating directory to store logs
            string logDirectory = Path.Combine(_loggerConfiguration.TextLoggerConfiguration.Directory, $"{now:MM-dd-yyyy}");

            // used to create log file itself
            string uniqueLogName = $"{_loggerConfiguration.TextLoggerConfiguration.Filename}-{now.ToString("HH-mm-ss")}";
            string baseLogName = Path.ChangeExtension(uniqueLogName, _loggerConfiguration.TextLoggerConfiguration.FileExtension);
            string filepath = Path.Combine(logDirectory, baseLogName);

            Directory.CreateDirectory(logDirectory);
            _ = Task.Run(() => LogAsync(filepath, _logQueue, _tokenSource.Token));
        }

        ~TextLogger()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            lock (_lock)
            {
                if (_disposed)
                {
                    return;
                }
            }

            _disposed = true;
            if (disposing)
            {
                // gets rid of managed resources by manually setting stopping token to true
                _tokenSource.Cancel();
                _tokenSource.Dispose();
            }

            // need to get rid of unmanaged resources
        }

        public static async void LogAsync(string filepath, BufferBlock<LogInformation> logQueue, CancellationToken token)
        {
            using FileStream fs = new FileStream(filepath, FileMode.CreateNew, FileAccess.Write, FileShare.Read);
            using var streamWriter = new StreamWriter(fs) { AutoFlush = true }; // using to dispose of the streamwriter at the end of the scope

            try
            {
                while (true)
                {
                    // pops the item off the logQueue and formats it 
                    var logItem = await logQueue.ReceiveAsync(token).ConfigureAwait(false);
                    string formattedMessage = FormatLogItem(logItem);
                    await streamWriter.WriteAsync(formattedMessage).ConfigureAwait(false);
                }
            } catch (OperationCanceledException)
            {
                throw new OperationCanceledException("Operation was cancelled.");
            }
        }

        private static string FormatLogItem(LogInformation logItem)
        {
            return $"[{logItem.Now:MM-dd-yyyy HH:mm:ss.fffffff}] [{logItem.ThreadName,-10}:{logItem.ThreadId:000}] " +
                $"[{logItem.LogLevel}] [{logItem.Message}]";
        }

        protected override void Log(LogLevel logLevel, string module, string message)
        {
            _logQueue.Post(new LogInformation(logLevel, module, message,
                DateTime.Now, Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.Name));
        }

        private readonly BufferBlock<LogInformation> _logQueue = new BufferBlock<LogInformation>();
        private readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();
        private readonly object _lock = new object();
        private bool _disposed = false;
    }
}

