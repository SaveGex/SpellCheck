using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.Configuration;

namespace Infrastructure.Configuration
{
    public class FileLoggerOptions
    {
        public LogLevel MinimalLoggingLevel { get; init; } = LogLevel.Information;
        public string LogFileDirectory
        {
            get;
            init
            {
                if (string.IsNullOrEmpty(value) && string.IsNullOrWhiteSpace(field))
                    field = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                else
                    field = value ?? field;
            }
        } = null!;
        public string LogFileName
        {
            get;
            init
            {
                field = value ?? field;
            }
        } = "applog.txt";

        public string FullLoggingPath => Path.Combine(LogFileDirectory, LogFileName);
    
    }
}