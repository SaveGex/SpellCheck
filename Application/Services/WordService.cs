
using Application.Interfaces;
using Application.ModelsDTO;
using AutoMapper;
using DomainData.Interfaces;
using DomainData.Models;

namespace Application.Services;

public class WordService : IWordService
{
    private IWordRepository WordRepository { get; init; }
    private IMapper Mapper { get; init; }

    public WordService(IWordRepository wordRepository, IMapper mapper)
    {
        WordRepository = wordRepository;
        Mapper = mapper;
    }

    public async Task<WordResponseDTO> CreateWordAsync(WordCreateDTO dto)
    {
        Word word = Mapper.Map<Word>(dto);
        Word result = await WordRepository.CreateWordAsync(word);
        return Mapper.Map<WordResponseDTO>(result);
    }

    public async Task<WordResponseDTO> UpdateWordAsync(WordUpdateDTO dto)
    {
        Word word = Mapper.Map<Word>(dto);
        Word result = await WordRepository.UpdateWordAsync(word);
        return Mapper.Map<WordResponseDTO>(word);
    }

    public async Task<WordResponseDTO> DeleteWordAsync(int wordId)
    {
        Word word = await WordRepository.GetWordByIdAsync(wordId);
        Word result = await WordRepository.DeleteWordAsync(word);
        return Mapper.Map<WordResponseDTO>(word);
    }

    public async Task<WordResponseDTO> GetWordByIdAsync(int wordId)
    {
        Word word = await WordRepository.GetWordByIdAsync(wordId);
        return Mapper.Map<WordResponseDTO>(word);
    }

    public async Task<IEnumerable<WordResponseDTO>> GetWordsByModuleIdAsync(int moduleId)
    {
        List<WordResponseDTO> wordsDTO = new List<WordResponseDTO>();
        foreach(Word word in await WordRepository.GetWordsByModuleIdAsync(moduleId))
        {
            wordsDTO.Add(Mapper.Map<WordResponseDTO>(word));
        }

        return wordsDTO;
    }
}
