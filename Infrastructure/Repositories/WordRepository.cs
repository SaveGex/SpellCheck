using AutoMapper;
using DomainData.Interfaces;
using DomainData.Models;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;

namespace DbManagerApi.Services.WordServices;


public class WordRepository : IWordRepository
{
    private readonly SpellTestDbContext _context;
    private readonly IMapper _mapper;
    public WordRepository(SpellTestDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }


    public async Task<bool> AnyAsync(Word word)
        => await _context.Words.FindAsync(word) is not null;


    public async Task<Word> CreateWordAsync(Word word)
    {
        _context.Words.Add(word);
        await _context.SaveChangesAsync();

        return word;
    }

    public async Task<IEnumerable<Word>> GetWordsSequenceAsync(string? propName, int? limit, int? moduleId, bool? reverse)
    {
        string orderBy = string.IsNullOrWhiteSpace(propName) ? nameof(Word.Id) : propName;
        int take = Math.Clamp(limit ?? 100, 0, 1000);
        int startId = moduleId ?? _context.Words.FirstOrDefault()?.ModuleId ?? throw new Exception("where is all words?");
        bool descending = reverse ?? false;

        IQueryable<Word> query = _context.Words
            .AsQueryable()
            .Where(w => w.Id >= startId);

        query = query.OrderByProperty(orderBy, descending);

        IEnumerable<Word> words = await query
            .Take(take)
            .ToListAsync();

        return words;
    }

    public async Task<Word> GetWordByIdAsync(int wordId)
    {
        Word? word = await _context.Words.FindAsync(wordId);

        if (word is null)
        {
            throw new Exception("Word not found.");
        }

        return word;

    }

    public Task<IEnumerable<Word>> GetWordsByModuleIdAsync(int moduleId)
    {
        IEnumerable<Word> words = _context.Words.Where(w => w.ModuleId == moduleId).AsEnumerable();

        if (!words.Any())
        {
            throw new Exception("No words found for the specified module ID.\n Or the module doesn't has any words");
        }

        return Task.FromResult(words);
    }

    public async Task<Word> UpdateWordAsync(Word word)
    {
        _context.Words.Update(word);
        await _context.SaveChangesAsync();

        return word;
    }
    public async Task<Word> DeleteWordAsync(Word word)
    {
        _context.Words.Add(word);
        await _context.SaveChangesAsync();

        return word;
    }

}
