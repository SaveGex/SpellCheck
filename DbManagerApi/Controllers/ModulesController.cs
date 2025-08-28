using DbManagerApi.Authentication.Handlers;
using DbManagerApi.Controllers.Filters;
using DbManagerApi.Controllers.Filters.FilterAttributes;
using DbManagerApi.Services;
using DbManagerApi.Services.Interfaces;
using FluentResults;
using Infrastructure;
using Infrastructure.Models;
using Infrastructure.Models.ModelsDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Security.Claims;

namespace DbManagerApi.Controllers;

[GuidModelConditionFilter]
[Route("api/[controller]")]
[ApiController]
public class ModulesController : ControllerBase
{
    private IModuleService ModuleService { get; set; }
    public ModulesController(SpellTestDbContext context)
    {
        ModuleService = new ModuleService(context);
    }


    [HttpGet]
    public async Task<IActionResult> GetAllModules(
        [FromQuery] string? propName,
        [FromQuery] int? limit,
        [FromQuery] int? moduleId,
        [FromQuery] bool? reverse,
        [FromQuery] int? wordsIncludeNumber)
    {
        Result<IEnumerable<ModuleResponseDTO>> result = await ModuleService.GetModulesSequenceAsync(propName, limit, moduleId, reverse, wordsIncludeNumber);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.Errors);
    }


    [HttpGet("{moduleId:int}")]
    public async Task<IActionResult> GetModuleById(int moduleId)
    {
        Result<ModuleResponseDTO> result = await ModuleService.GetModuleByIdAsync(moduleId);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.Errors);
    }


    [HttpPost]
    public async Task<IActionResult> CreateModule([FromBody] ModuleCreateDTO dto)
    {
        Result<ModuleResponseDTO> result = await ModuleService.CreateModuleAsync(dto);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.Errors);
    }


    [HttpPut("{moduleId:int}")]
    [UserOwnership("moduleId", "Modules")]
    public async Task<IActionResult> UpdateModule([FromBody] ModuleUpdateDTO dto, int moduleId)
    {
        Result<ModuleResponseDTO> result = await ModuleService.UpdateModuleAsync(dto, moduleId);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.Errors);
    }

    [HttpDelete("{moduleId:int}")]
    [UserOwnership("moduleId", "Modules")]
    public async Task<IActionResult> DeleteModule(int moduleId)
    {
        Result<ModuleResponseDTO> result = await ModuleService.DeleteModuleAsync(moduleId);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.Errors);
    }
}
