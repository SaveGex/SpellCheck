using CommunityToolkit.Mvvm.Input;
using SpellCheck.Services;
using SpellCheck.Services.Providers.Interfaces;
using SpellCheck.ViewModels;
using SpellCheck.Views;
using System.Windows.Input;

namespace SpellCheck
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(SplashPage), typeof(SplashPage));

            Navigated += OnShellNavigated;
        }

        private async void OnShellNavigated(object? sender, ShellNavigatedEventArgs e)
        {
            if (e.Current.Location.OriginalString.Contains("ExitPage"))
            {
                await Exit();
            }
        }

        private async Task Exit()
        {
            await Shell.Current.GoToAsync(nameof(SplashPage) + "?SignOut=true");
        }
    }
}
