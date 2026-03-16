


namespace DbManagerApi.Extentions
{
    public static class FileLoggerExtensions
    {
        extension(ILoggingBuilder loggingBuilder)
        {
            public ILoggingBuilder AddFileLogging()
            {
                var options = loggingBuilder.Services
                        .BuildServiceProvider()
                        .GetRequiredService<IConfiguration>()
                        .GetSection("Logging:FileLogger")
                        .Get<Infrastructure.Configuration.FileLoggerOptions>();

                if (options?.IsActive != true)
                    return loggingBuilder;

                loggingBuilder.Services.AddOptions<Infrastructure.Configuration.FileLoggerOptions>()
                    .BindConfiguration("Logging:FileLogger");
                loggingBuilder.Services.AddSingleton<ILoggerProvider, Infrastructure.Logging.Providers.FileLoggingProvider>();
                return loggingBuilder;
            }
        }
    }
}
