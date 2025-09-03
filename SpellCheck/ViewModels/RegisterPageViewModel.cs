using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Input;
using SpellCheck.Views;
using System.Windows.Input;
using SpellCheck.GeneratedApi;
using SpellCheck.Services;

namespace SpellCheck.ViewModels;

public partial class RegisterPageViewModel : ObservableObject
{
    public UserCreateDTO NewUser { get; set; } = new();

    public ICommand RegisterCommand { get; }
    public ICommand NavigateToLoginCommand { get; }
    public GeneratedClientApi ClientApi { get; init; }

    public RegisterPageViewModel(GeneratedClientApi clientApi)
    {
        RegisterCommand = new AsyncRelayCommand(OnRegister);
        NavigateToLoginCommand = new AsyncRelayCommand(OnNavigateToLogin);
        ClientApi = clientApi;
    }

    private async Task OnRegister()
    {
        try
        {
            await ClientApi.UsersPOSTAsync(NewUser);
        }
        catch(Exception ex)
        {
            Application.Current?.MainPage?.DisplayAlert("Error", ex.Message, "OK");
            return;
        }
        await OnNavigateToLogin();
    }
    
    private async Task OnNavigateToLogin()
    {
        if (Application.Current is null)
        {
            throw new Exception("Application.Current is null");
        }
        if (Application.Current.MainPage is null)
        {
            throw new Exception("Application.Current.MainPage is null");
        }
        await Application.Current.MainPage.Navigation.PopAsync();
    }
}
