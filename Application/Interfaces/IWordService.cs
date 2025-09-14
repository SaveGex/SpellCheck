
using Application.ModelsDTO;

namespace Application.Interfaces;

public interface IWordService
{
    Task<WordResponseDTO> CreateWordAsync(WordCreateDTO dto);
    Task<WordResponseDTO> UpdateWordAsync(WordUpdateDTO dto);
    Task<WordResponseDTO> DeleteWordAsync(int wordId);
    Task<WordResponseDTO> GetWordByIdAsync(int wordId);
    Task<IEnumerable<WordResponseDTO>> GetWordsByModuleIdAsync(int moduleId);
}
