
using SpellCheck.Services.Providers.Interfaces;

namespace SpellCheck.Services.Providers;

internal class CredentialsProvider : ICredentialsProvider
{
	private string? _email;

	public string? Email
	{
		get => _email;
		set
		{
            _email = value;
        }
	}

	private string? _phoneNumber;

	public string? PhoneNumber
	{
		get => _phoneNumber;
		set { _phoneNumber = value; }
	}

	private string? _password;

	public string? Password
	{
		get => _password;
		set { _password = value; }
	}


}
