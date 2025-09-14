using DbManagerApi.Controllers.Filters.FilterAttributes;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.ModelsDTO;

namespace DbManagerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WordsController : ControllerBase
{
    private IWordService WordService { get; set; }

    public WordsController(IWordService wordService)
    {
        WordService = wordService;
    }

    [HttpGet("{wordId:int}")]
    [ProducesResponseType(typeof(WordResponseDTO), StatusCodes.Status200OK)]
    public async Task<ActionResult<WordResponseDTO>> GetWordById(int wordId)
    {
        WordResponseDTO result;
        try
        {
            result = await WordService.GetWordByIdAsync(wordId);
        } 
        catch (Exception ex) 
        {
            return BadRequest(ex.Message);
        }
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(WordResponseDTO), StatusCodes.Status200OK)]
    public async Task<ActionResult<WordResponseDTO>> CreateWord([FromBody] WordCreateDTO dto)
    {
        WordResponseDTO result;
        try
        {
            result = await WordService.CreateWordAsync(dto);
        } 
        catch (Exception ex) 
        {
            return BadRequest(ex.Message);
        }
        return Ok(result);
    }


    [HttpPut("{wordId:int}")]
    [UserOwnership("wordId", "Words")]
    [ProducesResponseType(typeof(WordResponseDTO), StatusCodes.Status200OK)]
    public async Task<ActionResult<WordResponseDTO>> UpdateWord([FromBody] WordUpdateDTO dto, int wordId)
    {
        WordResponseDTO result;
        try
        {
            result = await WordService.UpdateWordAsync(dto);
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok(result);
    }

    [HttpDelete("{wordId:int}")]
    [UserOwnership("wordId", "Words")]
    [ProducesResponseType(typeof(WordResponseDTO), StatusCodes.Status200OK)]
    public async Task<ActionResult<WordResponseDTO>> DeleteWord(int wordId)
    {
        WordResponseDTO result;
        try
        {
            result = await WordService.DeleteWordAsync(wordId);
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok(result);
    }


    [HttpGet("/api/module/{moduleId:int}/[controller]")]
    [ProducesResponseType(typeof(WordResponseDTO), StatusCodes.Status200OK)]
    public async Task<ActionResult<WordResponseDTO>> GetWordsByModuleId(int moduleId)
    {
        IEnumerable<WordResponseDTO> result;
        try
        {
            result = await WordService.GetWordsByModuleIdAsync(moduleId);
        }
        catch (Exception ex) 
        {
            return BadRequest(ex.Message);
        }
        return Ok(result);
    }
}
