namespace SpellCheck.Handlers.Interfaces;

public interface IAuthorizationHandler
{
    public bool IsAuthorized();
    public Task<bool> IsAuthorizedAsync();
}
