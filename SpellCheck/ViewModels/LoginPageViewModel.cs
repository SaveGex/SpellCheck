using CommunityToolkit.Mvvm.Input;
using SpellCheck.Enums;
using SpellCheck.GeneratedApi;
using SpellCheck.Handlers.Interfaces;
using SpellCheck.Models;
using SpellCheck.Services;
using SpellCheck.Services.Providers.Interfaces;
using SpellCheck.Views;
using System.ComponentModel;
using System.Windows.Input;

namespace SpellCheck.ViewModels;
public class LoginPageViewModel : INotifyPropertyChanged
{
    private string _login = string.Empty;
    private string _errorMessage = string.Empty;
    private string _authorizationError = string.Empty;

    public string Login
    {
        get => _login;
        set
        {
            _login = value;
            OnPropertyChanged(nameof(Login));
        }
    }
    public string LoginErrorValidation
    {
        get => _errorMessage;
        set
        {
            _errorMessage = value;
            OnPropertyChanged(nameof(LoginErrorValidation));
        }
    }
    public string AuthorizationError
    {
        get => _authorizationError;
        set
        {
            _authorizationError = value;
            OnPropertyChanged(nameof(AuthorizationError));
        }
    }


    public User User { get; set; } = new User();

    /// <summary>
    /// More custom decision in relation to IMainPageProvider where you cannot access to an information about AuthorizationHandler.IsAuthorizedAsync() method.
    /// </summary>
    private IAuthorizationHandler AuthorizationHandler { get; set; }
    public ICommand LoginCommand { get; set; }
    public ICommand ForgotPasswordCommand { get; set; }
    public ICommand NavigateToRegisterCommand { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;

    public LoginPageViewModel(IAuthorizationHandler authorizationHandler)
    {
        LoginCommand = new AsyncRelayCommand(LoginAsync);
        ForgotPasswordCommand = new AsyncRelayCommand(ForgotPassword);
        NavigateToRegisterCommand = new AsyncRelayCommand(NavigateToRegister);

        AuthorizationHandler = authorizationHandler;
    }

    private async Task NavigateToRegister()
    {
        if (Application.Current is null)
        {
            throw new Exception("Application.Current is null");
        }
        if(Application.Current.MainPage is null)
        {
            throw new Exception("Application.Current.MainPage is null");
        }
        await Application.Current.MainPage.Navigation.PushAsync(new RegisterPage(ServiceHelper.GetService<RegisterPageViewModel>()));
    }

#pragma warning disable CS1998
    private async Task ForgotPassword()
        {
            throw new NotImplementedException();
        }
#pragma warning restore CS1998

    private async Task LoginAsync()
    {
        // Validation 
        switch (Determiner.DetermineLoginType(Login))
        {
            case LoginTypes.Email: User.Email = Login; break;
            case LoginTypes.PhoneNumber: User.Phone = Login; break;
            case LoginTypes.Indefined:
                {
                    LoginErrorValidation = "Email or number is not valid. Check it out and try again.";
                    AuthorizationError = "";
                }  return;
        }
        if(string.IsNullOrEmpty(User.Password))
        {
            LoginErrorValidation = "You forgot to type your password. Please enter your password and try again.";
            return;
        }
        await SetCredentials();

        if (!await AuthorizationHandler.IsAuthorizedAsync())
        {
            LoginErrorValidation = "";
            AuthorizationError = "Login or password is not valid. Check it out and try again.";
            return;
        }



        if(Application.Current is null)
        {
            throw new Exception("Application.Current is null");
        }
        Application.Current.MainPage = new AppShell(); 
    }

    private async Task SetCredentials()
    {
        switch (this.User)
        {
            case { Email: not null }: await SecureStorage.Default.SetAsync(MauiProgram.emailKeyWord, User.Email); break;
            case { Phone: not null }: await SecureStorage.Default.SetAsync(MauiProgram.phoneKeyWord, User.Phone); break;
            default: throw new Exception($"Email and phone number is null. Error caused error of binding to the User instance model in LoginPageViewModel");
        }

        await SecureStorage.Default.SetAsync(MauiProgram.passwordKeyWord, User.Password);

        await GeneratedClientApi.SetCredentialsAsync(Login, User.Password);
    }

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
