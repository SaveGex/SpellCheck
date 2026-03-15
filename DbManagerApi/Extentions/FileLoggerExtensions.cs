
using Infrastructure.Configuration;
using Infrastructure.Logging.Providers;


namespace DbManagerApi.Extentions
{
    public static class FileLoggerExtensions
    {
        extension(ILoggingBuilder loggingBuilder)
        {
            public ILoggingBuilder AddFileLogging()
            {
                loggingBuilder.Services.AddOptions<FileLoggerOptions>()
                    .BindConfiguration("Logging:FileLogger");

                loggingBuilder.Services.AddSingleton<ILoggerProvider, FileLoggingProvider>();

                return loggingBuilder;
            }
        }
    }
}
