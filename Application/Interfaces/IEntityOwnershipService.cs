namespace Application.Interfaces;

public interface IEntityOwnershipService
{
    Task<bool> IsUserOwnerAsync(int userId, int entityId, string entityName);
}
