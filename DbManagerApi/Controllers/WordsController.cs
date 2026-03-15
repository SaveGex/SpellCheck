using Application.Interfaces;
using Application.ModelsDTO;
using DbManagerApi.Controllers.Filters.FilterAttributes;
using Microsoft.AspNetCore.Mvc;

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
    [BindingAuthorId<WordCreateDTO>]
    [ProducesResponseType(typeof(WordResponseDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<WordResponseDTO>> CreateWord([FromBody] WordCreateDTO dto)
    {
        try
        {
            var result = await WordService.CreateWordAsync(dto);
            return CreatedAtAction(nameof(GetWordById), new { wordId = result.Id }, result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [HttpPut("{wordId:int}")]
    [UserOwnership("wordId", "Words")]
    [BindingAuthorId<WordUpdateDTO>]
    [ProducesResponseType(typeof(WordResponseDTO), StatusCodes.Status200OK)]
    public async Task<ActionResult<WordResponseDTO>> UpdateWord([FromBody] WordUpdateDTO dto, int wordId)
    {
        WordResponseDTO result;
        try
        {
            result = await WordService.UpdateWordAsync(dto);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok(result);
    }

    [HttpDelete("{wordId:int}")]
    [UserOwnership("wordId", "Words")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteWord(int wordId)
    {
        try
        {
            await WordService.DeleteWordAsync(wordId);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
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
