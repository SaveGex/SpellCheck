using DbManagerApi.Controllers.Filters.FilterAttributes;
using DbManagerApi.Services;
using DbManagerApi.Services.Abstracts;
using DbManagerApi.Services.ModuleServices;
using FluentResults;
using Infrastructure;
using Infrastructure.Models.ModelsDTO;
using Microsoft.AspNetCore.Mvc;
using MR.AspNetCore.Pagination;

namespace DbManagerApi.Controllers;

[GuidModelConditionFilter]
[Route("api/[controller]")]
[ApiController]
public class ModulesController : ControllerBase
{
    private ModuleServiceCursor ModuleService { get; set; }
    public ModulesController(SpellTestDbContext context, IPaginationService paginationService)
    {
        ModuleService = new ModuleServiceCursor(context, paginationService);
    }


    [HttpGet]
    [ProducesResponseType(typeof(KeysetPaginationResult<ModuleResponseDTO>), StatusCodes.Status200OK)]
    public async Task<ActionResult<KeysetPaginationResult<ModuleResponseDTO>>> GetAllModules(
        [FromQuery] string? after,
        [FromQuery] string? propName,
        [FromQuery] int? limit,
        [FromQuery] int? moduleId,
        [FromQuery] bool? reverse,
        [FromQuery] int? wordsIncludeNumber)
    {
        var result = await ModuleService.GetModulesKeySetPaginationAsync(after, propName, limit, moduleId, reverse, wordsIncludeNumber);
        if (result.IsSuccess)
        {
            Response.Headers.Append("After", await ModuleService.GetCursorBase64StringAsync(result.Value.Data.LastOrDefault(), propName));
            return Ok(result.Value);
        }
        return BadRequest(result.Errors);
    }


    [HttpGet("{moduleId:int}")]
    [ProducesResponseType(typeof(ModuleResponseDTO), StatusCodes.Status200OK)]
    public async Task<ActionResult<ModuleResponseDTO>> GetModuleById(int moduleId)
    {
        Result<ModuleResponseDTO> result = await ModuleService.GetEntityByIdAsync(moduleId);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.Errors);
    }


    [HttpPost]
    [ProducesResponseType(typeof(ModuleResponseDTO), StatusCodes.Status200OK)]
    public async Task<ActionResult<ModuleResponseDTO>> CreateModule([FromBody] ModuleCreateDTO dto)
    {
        Result<ModuleResponseDTO> result = await ModuleService.CreateEntityAsync(dto);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.Errors);
    }


    [HttpPut("{moduleId:int}")]
    [UserOwnership("moduleId", "Modules")]
    [ProducesResponseType(typeof(ModuleResponseDTO), StatusCodes.Status200OK)]
    public async Task<ActionResult<ModuleResponseDTO>> UpdateModule([FromBody] ModuleUpdateDTO dto, int moduleId)
    {
        Result<ModuleResponseDTO> result = await ModuleService.UpdateEntityAsync(dto, moduleId);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.Errors);
    }

    [HttpDelete("{moduleId:int}")]
    [UserOwnership("moduleId", "Modules")]
    [ProducesResponseType(typeof(ModuleResponseDTO), StatusCodes.Status200OK)]
    public async Task<ActionResult<ModuleResponseDTO>> DeleteModule(int moduleId)
    {
        Result<ModuleResponseDTO> result = await ModuleService.DeleteEntityAsync(moduleId);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.Errors);
    }
}
