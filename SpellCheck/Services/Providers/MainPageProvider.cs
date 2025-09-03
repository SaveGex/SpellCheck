using SpellCheck.GeneratedApi;
using SpellCheck.Handlers.Interfaces;
using SpellCheck.Services.Providers.Interfaces;
using SpellCheck.ViewModels;
using SpellCheck.Views;

namespace SpellCheck.Services;

/// <summary>
/// Depend on stored user credentials client send request by this credentials. 
/// And, if the user credentials is correct, change Application.Current.MainPage into AppShell as was by default.
/// Otherwise into LoginPage for set local the user credentials into correct.
/// </summary>
class MainPageProvider : IMainPageProviderAsync
{
    public GeneratedClientApi GeneratedClientApi { get; init; }
    public IAuthorizationHandler AuthorizationHandler { get; init; }
    public LoginPage LoginPage { get; init; }
    public MainPageProvider(GeneratedClientApi generatedClientApi, IAuthorizationHandler authorizationHandler, LoginPageViewModel loginPageViewModel)
    {
        GeneratedClientApi = generatedClientApi;
        AuthorizationHandler = authorizationHandler;
        LoginPage = new LoginPage(loginPageViewModel);
    }

    public async Task UserAuthorizeAsync()
    {
        bool isLogged = false;
        if (Application.Current is null)
        {
            throw new Exception("Application.Current instance is null");
        }
        try
        {
            await GeneratedClientApi.SetCredentialsAsync();
            isLogged = await AuthorizationHandler.IsAuthorizedAsync();
        }
        catch
        {
            Application.Current.MainPage = new NavigationPage(LoginPage);
        }

        if (!isLogged)
        {
            Application.Current.MainPage = new NavigationPage(LoginPage);
        }
        else
        {
            Application.Current.MainPage = new AppShell();
        }

    }

}
