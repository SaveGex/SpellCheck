using DomainData.Models;
using DomainData.Models.ModelsDTO;
using DomainData.Records;

namespace Application.Interfaces;

public interface IModuleService
{
    Task<ModuleResponseDTO> CreateModuleAsync(ModuleCreateDTO dto);
    Task<KeysetPaginationAfterResult<ModuleResponseDTO>> GetModulesKeysetPaginationAsync(string? after, string? propName, int? limit, int? Id, bool? reverse, int? wordsIncludeNumber);
    Task<ModuleResponseDTO> GetModuleByIdAsync(int moduleId);
    Task<ModuleResponseDTO> UpdateModuleAsync(ModuleUpdateDTO dto);
    Task<ModuleResponseDTO> DeleteModuleAsync(int moduleId);
}
