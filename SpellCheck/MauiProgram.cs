using SpellCheck.GeneratedApi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SpellCheck.Handlers;
using SpellCheck.Handlers.Interfaces;
using SpellCheck.Services;
using SpellCheck.Services.Providers;
using SpellCheck.Services.Providers.Interfaces;
using SpellCheck.ViewModels;
using SpellCheck.Views;
using System.ComponentModel;

namespace SpellCheck
{
    public static class MauiProgram
    {
        public const string emailKeyWord = "userEmail";
        public const string phoneKeyWord = "userPhone";
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

            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("Properties\\appsettings.json")
                .Build();

            builder.Configuration.AddConfiguration(configurationBuilder);

            //Providers
            builder.Services.AddSingleton<ICredentialsProvider, CredentialsProvider>();
            builder.Services.AddTransient<IMainPageProviderAsync, MainPageProvider>();

            //Handlers
            builder.Services.AddScoped<IAuthorizationHandler, BasicAuthorizationHandler>();

            //ViewModels
            builder.Services.AddTransient<LoginPageViewModel>();
            builder.Services.AddTransient<RegisterPageViewModel>();

            //Pages
            builder.Services.AddTransient<SplashPage>();

            //Others
            builder.Services.AddHttpClient<GeneratedClientApi>(client =>
            {
                string? baseUrl = builder.Configuration["applicationBaseUrl"];
                if (baseUrl == null)
                {
                    throw new Exception("Base url is null");
                }
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.Timeout = TimeSpan.FromSeconds(10);
            });

#if DEBUG
                builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        public static Task EraseCredentialEnvironments()
        {
            SecureStorage.Default.Remove(MauiProgram.phoneKeyWord);
            SecureStorage.Default.Remove(MauiProgram.passwordKeyWord);
            SecureStorage.Default.Remove(MauiProgram.emailKeyWord);
            return Task.CompletedTask;
        }
    }
}
