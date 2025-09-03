using SpellCheck.GeneratedApi;
using SpellCheck.Handlers.Interfaces;
using SpellCheck.Services.Providers.Interfaces;
using SpellCheck.Views;

namespace SpellCheck.Views
{
    [QueryProperty(nameof(SignOut), nameof(SignOut))]
    public partial class SplashPage : ContentPage
    {
        public IMainPageProviderAsync MainPageProvider { get; init; }
        public bool SignOut { get; set; } = false;
        public SplashPage(IMainPageProviderAsync mainPageProviderAsync)
        {
            InitializeComponent();
            MainPageProvider = mainPageProviderAsync;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (SignOut)
            {
                await MauiProgram.EraseCredentialEnvironments();
            }
            await MainPageProvider.UserAuthorizeAsync();
        }
    }
}
