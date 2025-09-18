using DomainData.Models;

namespace DomainData.Interfaces;

public interface IDifficultyLevelRepository
{
    Task<IEnumerable<DifficultyLevel>> GetLevelsAsync();
    Task<DifficultyLevel> CreateLevelAsync(DifficultyLevel level);
    Task<DifficultyLevel> UpdateLevelAsync(DifficultyLevel level);
    Task<DifficultyLevel> DeleteLevelAsync(DifficultyLevel level);
    Task<DifficultyLevel> GetLevelAsync(int levelId);
}
