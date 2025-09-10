using FluentResults;
using Infrastructure.Models.ModelsDTO;

namespace DbManagerApi.Services.Interfaces
{
    public interface IGetModulesSequenceAsync
    {
        Task<Result<IEnumerable<ModuleResponseDTO>>> GetModulesSequenceAsync(string? propName, int? limit, int? moduleId, bool? reverse, int? wordsIncludeNumber);
    }
}
