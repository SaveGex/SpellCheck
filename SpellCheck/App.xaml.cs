using SpellCheck.Views;

namespace SpellCheck
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            bool isLogged = false;

            if (!isLogged)
            {
                MainPage = new NavigationPage(new LoginPage());
                return;
            }
            MainPage = new AppShell();
        }
    }
}
