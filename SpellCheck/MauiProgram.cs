using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
namespace SpellCheck
{
    public static class MauiProgram
    {
        public const string usernameKeyWord = "username";
        public const string passwordKeyWord = "userPassword";
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
