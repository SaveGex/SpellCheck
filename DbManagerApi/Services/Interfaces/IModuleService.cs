using FluentResults;
using Infrastructure.Models.ModelsDTO;

namespace DbManagerApi.Services.Interfaces;

public interface IModuleService
{
    Task<Result<IEnumerable<ModuleResponseDTO>>> GetAllModulesAsync();
    Task<Result<ModuleResponseDTO>> GetModuleByIdAsync(int moduleId);
    Task<Result<ModuleResponseDTO>> CreateModuleAsync(ModuleCreateDTO dto);
    Task<Result<ModuleResponseDTO>> UpdateModuleAsync(ModuleUpdateDTO dto, int moduleId);
    Task<Result<ModuleResponseDTO>> DeleteModuleAsync(int moduleId);

}
