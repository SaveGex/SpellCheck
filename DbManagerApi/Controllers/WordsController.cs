using DbManagerApi.Controllers.Filters.FilterAttributes;
using DbManagerApi.Services;
using DbManagerApi.Services.Abstractions;
using FluentResults;
using Infrastructure;
using Infrastructure.Models.ModelsDTO;
using Microsoft.AspNetCore.Mvc;

namespace DbManagerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WordsController : ControllerBase
{
    private WordServiceAbstraction WordService { get; set; }

    public WordsController(SpellTestDbContext context)
    {
        WordService = new WordService(context);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllWords()
    {
        Result<IEnumerable<WordResponseDTO>> result = await WordService.GetAllEntitiesAsync();
        if (result.IsFailed)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result.Value);
    }

    [HttpGet("{wordId:int}")]
    public async Task<IActionResult> GetWordById(int wordId)
    {
        Result<WordResponseDTO> result = await WordService.GetEntityByIdAsync(wordId);
        if (result.IsFailed)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> CreateWord([FromBody] WordCreateDTO dto)
    {
        Result<WordResponseDTO> result = await WordService.CreateEntityAsync(dto);
        if (result.IsFailed)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result.Value);
    }


    [HttpPut("{wordId:int}")]
    [UserOwnership("wordId", "Words")]
    public async Task<IActionResult> UpdateWord([FromBody] WordUpdateDTO dto, int wordId)
    {
        Result<WordResponseDTO> result = await WordService.UpdateEntityAsync(dto, wordId);
        if (result.IsFailed)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result.Value);
    }

    [HttpDelete("{wordId:int}")]
    [UserOwnership("wordId", "Words")]
    public async Task<IActionResult> DeleteWord(int wordId)
    {
        Result<WordResponseDTO> result = await WordService.DeleteEntityAsync(wordId);
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
