using FluentResults;
using Infrastructure.Models.ModelsDTO;
using Microsoft.EntityFrameworkCore;
using MR.AspNetCore.Pagination;
using MR.EntityFrameworkCore.KeysetPagination;
using System.Reflection;

namespace DbManagerApi.Services.Interfaces;

public interface IGetKeySetPaginationAsync<TResponse>
{
    abstract Task<Result<KeysetPaginationResult<TResponse>>> GetKeysetPaginationAsync(string? after, string? propName, int? limit, int? moduleId, bool? reverse);
    abstract Task<string> GetCursorBase64StringAsync(TResponse? cursorElement, string? propName);
}
