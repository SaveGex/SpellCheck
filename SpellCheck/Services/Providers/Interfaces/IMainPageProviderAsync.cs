namespace SpellCheck.Services.Providers.Interfaces;

public interface IMainPageProviderAsync
{
    /// <summary>
    /// If user credentials which is stored locally is correct MainPage will be appointed into new AppShell() as was by default.
    /// Otherwise new LoginPage from DI
    /// </summary>
    /// <returns></returns>
    public Task UserAuthorizeAsync();
}
