using DbManagerApi.Services.Interfaces;
using Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace DbManagerApi.Services;

public class EntityOwnershipService : IEntityOwnershipService
{
    private readonly SpellTestDbContext _context;
    public EntityOwnershipService(SpellTestDbContext context)
    {
        _context = context;
    }
    public async Task<bool> IsUserOwnerAsync(int userId, int entityId, string entityName)
    {
        IEnumerable<IEntityType?> entityTypes = _context.Model
        .GetEntityTypes();
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

        object? entity = await _context.FindAsync(entityType.ClrType, entityId);

        if (entity is null)
        {
            throw new ArgumentException($"Entity with name '{entityName}' and ID '{entityId}' does not exist.");
        }

        if(entity.GetType().GetProperty("AuthorId") is not PropertyInfo AuthorId)
        {
            throw new ArgumentException($"Entity with name '{entityName}' does not have an 'AuthorId' property.");
        }

        if(AuthorId.GetValue(entity) is int authorId && authorId == userId)
        {
            return true;
        }

        return false;
    }
}
