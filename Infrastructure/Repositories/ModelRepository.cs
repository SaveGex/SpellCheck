
using DomainData;
using DomainData.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Infrastructure.Repositories;

public class ModelRepository : IModelRepository
{
    private SpellTestDbContext Context { get; init; }
    public ModelRepository(SpellTestDbContext context)
    {
        Context = context;
    }
    public IEnumerable<IEntityType?> GetEntityTypes()
    {
        return Context.Model.GetEntityTypes();
    }

    public async Task<object?> FindEntityAsync(Type clrType, params object?[]? keyValues)
    {
        return await Context.FindAsync(clrType, keyValues);
    }
}
