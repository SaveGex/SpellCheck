using ClientApi;
using CommunityToolkit.Mvvm.Input;
using SpellCheck.Enums;
using SpellCheck.Handlers.Interfaces;
using SpellCheck.Models;
using SpellCheck.Services;
using SpellCheck.Services.Providers.Interfaces;
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

    private IAuthorizationHandler AuthorizationHandler { get; set; }
    public ICommand LoginCommand { get; set; }
    public ICommand ForgotPasswordCommand { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;

    private ICredentialsProvider CredentialsProvider { get; set; }

    public LoginPageViewModel(IAuthorizationHandler authorizationHandler, ICredentialsProvider credentialsProvider)
    {
        LoginCommand = new AsyncRelayCommand(LoginAsync);
        ForgotPasswordCommand = new AsyncRelayCommand(ForgotPassword);
        AuthorizationHandler = authorizationHandler;
        CredentialsProvider = credentialsProvider;
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
            throw new Exception("App.Current is null");
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
