using Infrastructure.Configuration;
using Infrastructure.Logging.Interfaces;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Logging.Loggers
{
    internal class FileLogger : IFileLogger
    {
        private FileLoggerOptions LoggerOptions { get; set; }
        public FileLogger(FileLoggerOptions fileLoggerOptions)
        {
            LoggerOptions = fileLoggerOptions;
        }


        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= LoggerOptions.MinimalLoggingLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;

            var message = formatter(state, exception);
            File.AppendAllText(LoggerOptions.FullLoggingPath, $"[{logLevel}] {message} {exception?.Message}\n");
        }
    }
}
