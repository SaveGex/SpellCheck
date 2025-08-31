
using SpellCheck;
using SpellCheck.Services.Converters;
using System.Text;

namespace ClientApi;

public partial class GeneratedClientApi
{
    public static string? login;
    public static string? password;

    partial void PrepareRequest(HttpClient client, HttpRequestMessage request, StringBuilder urlBuilder)
    {
        //login:password
        StringBuilder credentials = new StringBuilder();

        credentials.Append(login ?? "");
        
        credentials.Append(':');

        credentials.Append(password ?? "");

        request.Headers.Add("Authorization", $" Basic {Base64Converter.StringToBase64(credentials.ToString())}");
    }

    public static async Task SetCredentialsAsync()
    {
        var email = await SecureStorage.Default.GetAsync(MauiProgram.emailKeyWord);
        var phone = await SecureStorage.Default.GetAsync(MauiProgram.phoneKeyWord);

        string? login = (email, phone) switch
        {
            (not null, _) => email,
            (null, not null) => phone,
            _ => null
        };

        GeneratedClientApi.login = login;
        GeneratedClientApi.password = await SecureStorage.Default.GetAsync(MauiProgram.passwordKeyWord);
    }

    public static Task SetCredentialsAsync(string login, string password)
    {
        GeneratedClientApi.login = login;
        GeneratedClientApi.password = password;
        return Task.CompletedTask;
    }

    public static void SetCredentials(string login, string password)
    {
        GeneratedClientApi.login = login;
        GeneratedClientApi.password = password; 
    }
}
