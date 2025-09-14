
using Microsoft.EntityFrameworkCore.Metadata;

namespace DomainData.Interfaces;

public interface IModelRepository
{
    IEnumerable<IEntityType?> GetEntityTypes();
    Task<object?> FindEntityAsync(Type clrType, params object?[]? keyValues);
}
