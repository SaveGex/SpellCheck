namespace ClientApi;

public class ManagerClientApi
{
    private readonly GeneratedClientApi _apiClient;
    public ManagerClientApi(GeneratedClientApi apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task AuthAsync(string username, string password)
    {

    }
}
