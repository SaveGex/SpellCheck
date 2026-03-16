


using System.Configuration;

namespace DbManagerApi.Extentions
{
    public static class FileLoggerExtensions
    {
        extension(ILoggingBuilder loggingBuilder)
        {
            public ILoggingBuilder AddFileLogging(IConfiguration configuration)
            {
                var isActive = configuration
                    .GetSection("Logging:FileLogger")
                    .GetValue<bool>("IsActive");

                if (!isActive)
                    return loggingBuilder;

                loggingBuilder.Services
                    .AddOptions<Infrastructure.Configuration.FileLoggerOptions>()
                    .BindConfiguration("Logging:FileLogger");
                loggingBuilder.Services
                    .AddSingleton<ILoggerProvider, Infrastructure.Logging.Providers.FileLoggingProvider>();
                return loggingBuilder;
            }
        }
    }
}
