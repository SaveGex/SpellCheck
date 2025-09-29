using DomainData.Models;

namespace DomainData.Interfaces;

public interface IWordRepository
{
    Task<bool> AnyAsync(Word word);

    Task<Word> CreateWordAsync(Word word);
    Task<IEnumerable<Word>> GetWordsByModuleIdAsync(int moduleId);
    Task<Word> GetWordByIdAsync(int wordId);
    Task<Word> UpdateWordAsync(Word word);
    Task<Word> DeleteWordAsync(Word word);
}
