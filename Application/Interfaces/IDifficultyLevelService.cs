using Application.ModelsDTO;

namespace Application.Interfaces;

public interface IDifficultyLevelService
{
    Task<IEnumerable<DifficultyLevelResponseDTO>> GetLevelsAsync();
    Task<DifficultyLevelResponseDTO> CreateLevelAsync(DifficultyLevelCreateDTO dto);
    Task<DifficultyLevelResponseDTO> UpdateLevelAsync(DifficultyLevelUpdateDTO dto);
    Task<DifficultyLevelResponseDTO> DeleteLevelAsync(int levelId);
}
