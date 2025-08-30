using ClientApi;
using Microsoft.Maui.Storage;
using SpellCheck.Handlers.Interfaces;

namespace SpellCheck.Handlers;

public class BasicAuthorizationHandler : IAuthorizationHandler
{
    public readonly GeneratedClientApi Client;
    public BasicAuthorizationHandler(GeneratedClientApi apiClient)
    {
        Client = apiClient;
    }

    public async bool IsAuthorized()
    {
        string? username = await SecureStorage.Default.GetAsync(MauiProgram.username);
        string? password = await SecureStorage.Default.GetAsync(MauiProgram.password);
        if(username != null && password != null)
        {
            return false;
        }


        
    }

    public Task<bool> IsAuthorizedAsync()
    {
        throw new NotImplementedException();
    }
}
