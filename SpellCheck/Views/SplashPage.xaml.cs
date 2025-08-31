using ClientApi;
using SpellCheck.Handlers.Interfaces;
using SpellCheck.Views;

namespace SpellCheck.Views
{
    public partial class SplashPage : ContentPage
    {
        private readonly IAuthorizationHandler _authorizationHandler;
        private readonly LoginPage _loginPage;

        public SplashPage(IAuthorizationHandler authorizationHandler, LoginPage loginPage)
        {
            InitializeComponent();
            _authorizationHandler = authorizationHandler;
            _loginPage = loginPage;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            bool isLogged = false;
            if (Application.Current is null)
            {
                throw new Exception("Application.Current instance is null");
            }
            try
            {
                await GeneratedClientApi.SetCredentialsAsync();
                isLogged = await _authorizationHandler.IsAuthorizedAsync();
            }
            catch
            {
                Application.Current.MainPage = new NavigationPage(_loginPage);
            }

            if (!isLogged)
            {
                Application.Current.MainPage = new NavigationPage(_loginPage);
            }
            else
            {
                Application.Current.MainPage = new AppShell();
            }
        }
    }
}
