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

    public async Task<bool> IsAuthorizedAsync()
    {
        try
        {
            await Client.AuthAsync();
            return true;
        }
        catch (ApiException)
        {
            //(401/403)
            return false;
        }
        catch (HttpRequestException ex)
        {
            System.Diagnostics.Debug.WriteLine($"Network error: {ex.Message}");
            return false;
        }
        catch (TaskCanceledException ex)
        {
            // timeout
            System.Diagnostics.Debug.WriteLine($"Request timeout: {ex.Message}");
            return false;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Unexpected error: {ex}");
            return false;
        }

    }
}
