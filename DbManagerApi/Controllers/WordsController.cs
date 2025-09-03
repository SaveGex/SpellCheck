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
    [ProducesResponseType(typeof(IEnumerable<WordResponseDTO>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<WordResponseDTO>>> GetAllWords()
    {
        Result<IEnumerable<WordResponseDTO>> result = await WordService.GetAllEntitiesAsync();
        if (result.IsFailed)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result.Value);
    }

    [HttpGet("{wordId:int}")]
    [ProducesResponseType(typeof(WordResponseDTO), StatusCodes.Status200OK)]
    public async Task<ActionResult<WordResponseDTO>> GetWordById(int wordId)
    {
        Result<WordResponseDTO> result = await WordService.GetEntityByIdAsync(wordId);
        if (result.IsFailed)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result.Value);
    }

    [HttpPost]
    [ProducesResponseType(typeof(WordResponseDTO), StatusCodes.Status200OK)]
    public async Task<ActionResult<WordResponseDTO>> CreateWord([FromBody] WordCreateDTO dto)
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
    [ProducesResponseType(typeof(WordResponseDTO), StatusCodes.Status200OK)]
    public async Task<ActionResult<WordResponseDTO>> UpdateWord([FromBody] WordUpdateDTO dto, int wordId)
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
    [ProducesResponseType(typeof(WordResponseDTO), StatusCodes.Status200OK)]
    public async Task<ActionResult<WordResponseDTO>> DeleteWord(int wordId)
    {
        Result<WordResponseDTO> result = await WordService.DeleteEntityAsync(wordId);
        if (result.IsFailed)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result.Value);
    }


    [HttpGet("/api/module/{moduleId:int}/[controller]")]
    [ProducesResponseType(typeof(WordResponseDTO), StatusCodes.Status200OK)]
    public async Task<ActionResult<WordResponseDTO>> GetWordsByModuleId(int moduleId)
    {
        Result<IEnumerable<WordResponseDTO>> result = await WordService.GetWordsByModuleIdAsync(moduleId);
        if (result.IsFailed)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result.Value);
    }
}
