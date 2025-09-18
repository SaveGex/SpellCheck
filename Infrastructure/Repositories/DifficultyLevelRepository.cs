
using DomainData;
using DomainData.Interfaces;
using DomainData.Models;

namespace Infrastructure.Repositories;

public class DifficultyLevelRepository : IDifficultyLevelRepository
{

    public SpellTestDbContext Context { get; init; }

    public DifficultyLevelRepository(SpellTestDbContext context)
    {
        Context = context;
    }

    public async Task<DifficultyLevel> CreateLevelAsync(DifficultyLevel level)
    {
        var result = Context.DifficultyLevels.Add(level).Entity;
        await Context.SaveChangesAsync();
        return result;
    }

    public async Task<DifficultyLevel> DeleteLevelAsync(DifficultyLevel level)
    {
        var result = Context.DifficultyLevels.Remove(level).Entity;
        await Context.SaveChangesAsync();
        return result;
    }

    public Task<IEnumerable<DifficultyLevel>> GetLevelsAsync()
    {
        return Task.FromResult(Context.DifficultyLevels.AsEnumerable());
    }

    public async Task<DifficultyLevel> UpdateLevelAsync(DifficultyLevel level)
    {
        var result = Context.DifficultyLevels.Update(level).Entity;
        await Context.SaveChangesAsync();
        return result;
    }

    public async Task<DifficultyLevel> GetLevelAsync(int levelId)
    {
        var result = await Context.DifficultyLevels.FindAsync(levelId);
        if (result == null)
        {
            throw new Exception($"Difficulty level with id '{levelId}' does not found");
        }
        return result;
    }
}
