using SpellCheck.Handlers.Interfaces;
using SpellCheck.Views;
using Microsoft.Extensions.DependencyInjection;

namespace SpellCheck
{
    public partial class App : Application
    {
        public App(SplashPage splashPage)
        {
            InitializeComponent();

            MainPage = splashPage;
        }
    }
}
