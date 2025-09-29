using Application.Interfaces;
using DomainData.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Reflection;
namespace DbManagerApi.Services;

public class EntityOwnershipService : IEntityOwnershipService
{

    private IModelRepository ModelRepository { get; init; }


    public EntityOwnershipService(IModelRepository modelRepository)
    {
        ModelRepository = modelRepository;
    }


    public async Task<bool> IsUserOwnerAsync(int userId, int entityId, string entityName)
    {
        IEnumerable<IEntityType?> entityTypes = ModelRepository.GetEntityTypes();
        IEntityType? entityType;

        entityType = entityTypes.FirstOrDefault(e =>
        (
            e is not null ?
            (e.ClrType.Name.ToLower() + 's' == entityName.ToLower())
            : false)
        );

        if (entityType is null)
        {
            throw new ArgumentException($"Entity with name '{entityName}' does not exist in the current context.");
        }

        object? entity = await ModelRepository.FindEntityAsync(entityType.ClrType, entityId);

        if (entity is null)
        {
            throw new ArgumentException($"Entity with name '{entityName}' and ID '{entityId}' does not exist.");
        }

        if (entity.GetType().GetProperty("AuthorId") is not PropertyInfo AuthorId)
        {
            throw new ArgumentException($"Entity with name '{entityName}' does not have an 'AuthorId' property.");
        }

        if (AuthorId.GetValue(entity) is int authorId && authorId == userId)
        {
            return true;
        }

        return false;
    }
}
