
using DomainData.Models;
using DomainData.Records;

namespace DomainData.Interfaces;

public interface IModuleRepository
{
    Task<bool> AnyAsync(Guid identifier);


    Task<Module> CreateModuleAsync(Module module);

    Task<KeysetPaginationAfterResult<Module>> GetModulesKeySetPaginationAsync(string? after, string? propName, int? limit, int? moduleId, bool? reverse, int? wordsIncludeNumber);
    Task<Module> GetModuleByIdAsync(int moduleId);
    Task<Module> UpdateModuleAsync(Module module);
    Task<Module> DeleteModuleAsync(Module module);

}
