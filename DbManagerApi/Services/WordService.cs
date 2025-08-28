using DbManagerApi.Controllers.Filters.FilterAttributes;
using DbManagerApi.Services.Interfaces;
using FluentResults;
using Infrastructure;
using Infrastructure.Models;
using Infrastructure.Models.ModelsDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DbManagerApi.Services;


public class WordService : IWordService
{
    private readonly SpellTestDbContext _context;
    public WordService(SpellTestDbContext context)
    {
        _context = context;
    }

    public static WordResponseDTO MapToDTO(Word word)
    {
        return new WordResponseDTO
        {
            Id = word.Id,
            AuthorId = word.AuthorId,
            ModuleId = word.ModuleId,
            Expression = word.Expression,
            Meaning = word.Meaning,
            DifficultyId = word.DifficultyId,
            CreatedAt = word.CreatedAt
        };
    }

    
    public async Task<Result<WordResponseDTO>> CreateWordAsync(WordCreateDTO dto)
    {
        Word word = new Word()
        {
            AuthorId = dto.AuthorId,
            ModuleId = dto.ModuleId,
            Expression = dto.Expression,
            Meaning = dto.Meaning,
            DifficultyId = dto.DifficultyId
        };
        

        if (await _context.Words.AnyAsync(w =>
            (
                w.ModuleId == word.ModuleId &&
                w.Expression.ToLower() == word.Expression.ToLower() &&
                w.Meaning.ToLower() == word.Meaning.ToLower()
            )
        ))
        {
            return Result.Fail("The same Word in the same Module already exists.");
        }

        _context.Words.Add(word);
        _context.SaveChanges();

        return Result.Ok(MapToDTO(word));
    }

    [UserOwnership("wordId", "Words")]
    public async Task<Result<WordResponseDTO>> DeleteWordAsync(int wordId)
    {
        if(wordId <= 0)
        {
            return Result.Fail<WordResponseDTO>("Invalid word ID.");
        }

        Word? word = await _context.Words.FindAsync(wordId);

        if (word is null)
        {
            return Result.Fail<WordResponseDTO>("Word not found.");
        }

        _context.Entry(word).State = EntityState.Deleted;
        await _context.SaveChangesAsync();
        return Result.Ok(MapToDTO(word));
    }

    [Authorize(Roles = $"{RoleNames.Manager}, {RoleNames.Admin}")]
    public async Task<Result<IEnumerable<WordResponseDTO>>> GetAllWordsAsync()
    {
        if(await _context.Words.AnyAsync() is false)
        {
            return Result.Fail<IEnumerable<WordResponseDTO>>("No words found.");
        }

        return Result.Ok(_context.Words.Select(w => MapToDTO(w)).AsEnumerable());
    }

    public async Task<Result<WordResponseDTO>> GetWordByIdAsync(int wordId)
    {
        Word? word = await _context.Words.FindAsync(wordId);
        
        if(word is null)
        {
            return Result.Fail("Word not found.");
        }

        return Result.Ok(MapToDTO(word));

    }

    public Task<Result<IEnumerable<WordResponseDTO>>> GetWordsByModuleIdAsync(int moduleId)
    {
        IEnumerable<WordResponseDTO> words = _context.Words.Where(w => w.ModuleId == moduleId)
            .Select(w => MapToDTO(w)).AsEnumerable();

        if (!words.Any())
        {
            return Task.FromResult<Result<IEnumerable<WordResponseDTO>>>(Result.Fail("No words found for the specified module ID.\n Or the module doesn't has any words"));
        }

        return Task.FromResult<Result<IEnumerable<WordResponseDTO>>>(Result.Ok(words));
    }

    [UserOwnership("wordId", "Words")]
    public async Task<Result<WordResponseDTO>> UpdateWordAsync(WordUpdateDTO dto, int wordId)
    {
        Word? word = await _context.Words.FindAsync(wordId);

        if(word is null)
        {
            return Result.Fail<WordResponseDTO>("Word not found.");
        }

        _context.Entry(word).CurrentValues.SetValues(dto);
        _context.Entry(word).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return Result.Ok(MapToDTO(word));
    }
}
