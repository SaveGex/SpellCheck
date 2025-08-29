using Azure;
using FluentResults;
using Infrastructure.Models.ModelsDTO;

namespace DbManagerApi.Services.Interfaces
{
    public interface IGetModulesSequenceAsync
    {
        Task<Result<IEnumerable<ModuleResponseDTO>>> GetEntitiesSequenceAsync(string? propName, int? limit, int? moduleId, bool? reverse, int? wordsIncludeNumber);
    }
}
