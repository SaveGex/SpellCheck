using Infrastructure.Configuration;
using Infrastructure.Logging.Loggers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Logging.Providers
{
    public class FileLoggingProvider : ILoggerProvider
    {
        public FileLoggerOptions LoggerOptions { get; init; }

        public FileLoggingProvider(IOptions<FileLoggerOptions> fileLoggerOptions)
        {
            LoggerOptions = fileLoggerOptions.Value;
        }


        public ILogger CreateLogger(string categoryName)
        {
            var logger = new FileLogger(LoggerOptions);
            return logger;
        }

        public void Dispose() { }

    }
}
