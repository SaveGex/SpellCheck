using Application.Interfaces;
using Application.ModelsDTO;
using AutoMapper;
using DomainData.Interfaces;
using DomainData.Models;

namespace Application.Services;

public class DifficultyLevelService : IDifficultyLevelService
{
    public IDifficultyLevelRepository DifficultyLevelRepository { get; init; }

    public IMapper Mapper { get; init; }


    public DifficultyLevelService(IDifficultyLevelRepository difficultyLevelRepository, IMapper mapper)
    {
        DifficultyLevelRepository = difficultyLevelRepository;
        Mapper = mapper;
    }

    public async Task<DifficultyLevelResponseDTO> CreateLevelAsync(DifficultyLevelCreateDTO dto)
    {
        DifficultyLevel result = await DifficultyLevelRepository.CreateLevelAsync(
            Mapper.Map<DifficultyLevel>(dto));
        return Mapper.Map<DifficultyLevelResponseDTO>(result);
    }

    public async Task<DifficultyLevelResponseDTO> DeleteLevelAsync(int levelId)
    {
        DifficultyLevel level = await DifficultyLevelRepository.GetLevelAsync(levelId);
        DifficultyLevel result = await DifficultyLevelRepository.DeleteLevelAsync(level);
        return Mapper.Map<DifficultyLevelResponseDTO>(result);
    }

    public async Task<IEnumerable<DifficultyLevelResponseDTO>> GetLevelsAsync()
    {
        var levels = await DifficultyLevelRepository.GetLevelsAsync();
        List<DifficultyLevelResponseDTO> result = new List<DifficultyLevelResponseDTO>();
        foreach (var level in levels)
        {
            result.Add(
                Mapper.Map<DifficultyLevelResponseDTO>(level));
        }
        return result;
    }

    public async Task<DifficultyLevelResponseDTO> UpdateLevelAsync(DifficultyLevelUpdateDTO dto)
    {
        DifficultyLevel level = Mapper.Map<DifficultyLevel>(dto);
        DifficultyLevel result = await DifficultyLevelRepository.UpdateLevelAsync(level);
        return Mapper.Map<DifficultyLevelResponseDTO>(result);
    }
}
