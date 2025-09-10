using FluentResults;
using Infrastructure.Models.ModelsDTO;
using Microsoft.EntityFrameworkCore;
using MR.AspNetCore.Pagination;
using System.Reflection;

namespace DbManagerApi.Services.Interfaces;

public interface IGetModulesKeySetPaginationAsync
{
    abstract Task<Result<KeysetPaginationResult<ModuleResponseDTO>>> GetModulesKeySetPaginationAsync(string? after, string? propName, int? limit, int? moduleId, bool? reverse, int? wordsIncludeNumber);
    abstract Task<string> GetCursorBase64StringAsync(ModuleResponseDTO? cursorElement, string? propName);
}
