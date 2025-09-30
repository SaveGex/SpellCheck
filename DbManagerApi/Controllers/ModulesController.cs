using Application.Interfaces;
using Application.ModelsDTO;
using DbManagerApi.Controllers.Filters.FilterAttributes;
using DomainData.Records;
using Microsoft.AspNetCore.Mvc;
using MR.AspNetCore.Pagination;

namespace DbManagerApi.Controllers;

[GuidModelConditionFilter]
[Route("api/[controller]")]
[ApiController]
public class ModulesController : ControllerBase
{
    private IModuleService ModuleService { get; init; }
    public ModulesController(IModuleService moduleService)
    {
        ModuleService = moduleService;
    }


    [HttpGet]
    [ProducesResponseType(typeof(KeysetPaginationResult<ModuleResponseDTO>), StatusCodes.Status200OK)]
    public async Task<ActionResult<KeysetPaginationAfterResult<ModuleResponseDTO>>> GetAllModules(
        [FromQuery] string? after,
        [FromQuery] string? propName,
        [FromQuery] int? limit,
        [FromQuery] int? moduleId,
        [FromQuery] bool? reverse,
        [FromQuery] int? wordsIncludeNumber)
    {
        KeysetPaginationAfterResult<ModuleResponseDTO> result; try
        {
            result = await ModuleService.GetModulesKeysetPaginationAsync(after, propName, limit, moduleId, reverse, wordsIncludeNumber);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        Response.Headers.Append("After", result.After);
        return Ok(result);
    }


    [HttpGet("{moduleId:int}")]
    [ProducesResponseType(typeof(ModuleResponseDTO), StatusCodes.Status200OK)]
    public async Task<ActionResult<ModuleResponseDTO>> GetModuleById(int moduleId)
    {
        ModuleResponseDTO result;
        try
        {
            result = await ModuleService.GetModuleByIdAsync(moduleId);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok(result);
    }


    [HttpPost]
    [ProducesResponseType(typeof(ModuleResponseDTO), StatusCodes.Status200OK)]
    public async Task<ActionResult<ModuleResponseDTO>> CreateModule([FromBody] ModuleCreateDTO dto)
    {
        ModuleResponseDTO result;
        try
        {
            result = await ModuleService.CreateModuleAsync(dto);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok(result);
    }


    [HttpPut("{moduleId:int}")]
    [UserOwnership("moduleId", "Modules")]
    [ProducesResponseType(typeof(ModuleResponseDTO), StatusCodes.Status200OK)]
    public async Task<ActionResult<ModuleResponseDTO>> UpdateModule([FromBody] ModuleUpdateDTO dto, int moduleId)
    {
        ModuleResponseDTO result;
        try
        {
            result = await ModuleService.UpdateModuleAsync(dto);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok(result);
    }

    [HttpDelete("{moduleId:int}")]
    [UserOwnership("moduleId", "Modules")]
    [ProducesResponseType(typeof(ModuleResponseDTO), StatusCodes.Status200OK)]
    public async Task<ActionResult<ModuleResponseDTO>> DeleteModule(int moduleId)
    {
        ModuleResponseDTO result;
        try
        {
            result = await ModuleService.DeleteModuleAsync(moduleId);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok(result);
    }
}
