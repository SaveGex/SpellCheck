namespace SpellCheck.Services.Providers.Interfaces;

public interface ICredentialsProvider
{
    string? Email { get; set; }
    string? PhoneNumber { get; set; }
    string? Password { get; set; }
}
