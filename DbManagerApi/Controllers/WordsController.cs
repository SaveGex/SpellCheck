using DbManagerApi.Services;
using DbManagerApi.Services.Interfaces;
using FluentResults;
using Infrastructure;
using Infrastructure.Models;
using Infrastructure.Models.ModelsDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DbManagerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WordsController : ControllerBase
{
    private IWordService WordService { get; set; }

    public WordsController(SpellTestDbContext context)
    {
        WordService = new WordService(context);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllWords()
    {
        Result<IEnumerable<WordResponseDTO>> result = await WordService.GetAllWordsAsync();
        if (result.IsFailed)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result.Value);
    }

    [HttpGet("{wordId:int}")]
    public async Task<IActionResult> GetWordById(int wordId)
    {
        Result<WordResponseDTO> result = await WordService.GetWordByIdAsync(wordId);
        if (result.IsFailed)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> CreateWord([FromBody] WordCreateDTO dto)
    {
        Result<WordResponseDTO> result = await WordService.CreateWordAsync(dto);
        if (result.IsFailed)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result.Value);
    }


    [HttpPut("{wordId:int}")]
    public async Task<IActionResult> UpdateWord([FromBody] WordUpdateDTO dto, int wordId)
    {
        Result<WordResponseDTO> result = await WordService.UpdateWordAsync(dto, wordId);
        if (result.IsFailed)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result.Value);
    }

    [HttpDelete("{wordId:int}")]
    public async Task<IActionResult> DeleteWord(int wordId)
    {
        Result<WordResponseDTO> result = await WordService.DeleteWordAsync(wordId);
        if (result.IsFailed)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result.Value);
    }


    [HttpGet("/api/module/{moduleId:int}/[controller]")]
    public async Task<IActionResult> GetWordsByModuleId(int moduleId)
    {
        Result<IEnumerable<WordResponseDTO>> result = await WordService.GetWordsByModuleIdAsync(moduleId);
        if (result.IsFailed)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result.Value);
    }
}
