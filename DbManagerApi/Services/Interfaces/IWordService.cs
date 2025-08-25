using FluentResults;
using Infrastructure.Models.ModelsDTO;

namespace DbManagerApi.Services.Interfaces;

public interface IWordService
{
    Task<Result<IEnumerable<WordResponseDTO>>> GetAllWordsAsync();
    Task<Result<WordResponseDTO>> GetWordByIdAsync(int wordId);
    Task<Result<WordResponseDTO>> CreateWordAsync(WordCreateDTO dto);
    Task<Result<WordResponseDTO>> UpdateWordAsync(WordUpdateDTO dto, int wordId);
    Task<Result<WordResponseDTO>> DeleteWordAsync(int wordId);
    Task<Result<IEnumerable<WordResponseDTO>>> GetWordsByModuleIdAsync(int moduleId);
}
