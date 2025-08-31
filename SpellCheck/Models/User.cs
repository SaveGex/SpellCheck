

using System.ComponentModel;

namespace SpellCheck.Models;
public class User : INotifyPropertyChanged
{
    private int? _id;
    public int? Id
    {
        get => _id;
        set 
        { 
            _id = value; 
            OnPropertyChanged(nameof(Id));
        }
    }

    private string? _phone;
    public string? Phone
    {
        get => _phone;
        set { _phone = value; }
    }

    private string? _email;
    public string? Email
    {
        get => _email;
        set 
        {
            _email = value;
            OnPropertyChanged(nameof(Email));
        }
    }


    private string _password = string.Empty;
    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged(nameof(Password));
        }
    } 

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
