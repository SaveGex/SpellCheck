using DbManagerApi.Services.Interfaces;
using FluentResults;
using Infrastructure.Models.ModelsDTO;

namespace DbManagerApi.Services.Abstracts
{
    public abstract class ModuleServiceAbstract :
        IGetModulesSequenceAsync,
        ICreateEntityAsync<ModuleResponseDTO, ModuleCreateDTO>,
        IDeleteEntityAsync<ModuleResponseDTO>,
        IGetEntityByIdAsync<ModuleResponseDTO>,
        IUpdateEntityAsync<ModuleResponseDTO, ModuleUpdateDTO>
    {
        public abstract Task<Result<IEnumerable<ModuleResponseDTO>>> GetEntitiesSequenceAsync(string? propName, int? limit, int? moduleId, bool? reverse, int? wordsIncludeNumber);
        public abstract Task<Result<ModuleResponseDTO>> CreateEntityAsync(ModuleCreateDTO dto);
        public abstract Task<Result<ModuleResponseDTO>> DeleteEntityAsync(int moduleId);
        public abstract Task<Result<ModuleResponseDTO>> GetEntityByIdAsync(int moduleId);
        public abstract Task<Result<ModuleResponseDTO>> UpdateEntityAsync(ModuleUpdateDTO dto, int moduleId);
    }
}
