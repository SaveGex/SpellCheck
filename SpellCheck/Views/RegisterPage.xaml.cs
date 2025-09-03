using CommunityToolkit.Mvvm.Messaging;
using SpellCheck.ViewModels;

namespace SpellCheck.Views;

public partial class RegisterPage : ContentPage
{
    public RegisterPage(RegisterPageViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}