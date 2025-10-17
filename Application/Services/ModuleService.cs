using Application.Interfaces;
using Application.ModelsDTO;
using AutoMapper;
using DomainData.Interfaces;
using DomainData.Models;
using DomainData.Records;

namespace Application.Services;

public class ModuleService : IModuleService
{

    private IModuleRepository ModuleRepository { get; init; }
    private IMapper Mapper { get; init; }

    public ModuleService(IModuleRepository moduleRepository, IMapper mapper)
    {
        this.ModuleRepository = moduleRepository;
        Mapper = mapper;
    }

    public async Task<ModuleResponseDTO> CreateModuleAsync(ModuleCreateDTO dto)
    {
        Module? module = Mapper.Map<Module>(dto);
        if (await ModuleRepository.AnyAsync(module.Identifier))
        {
            throw new Exception("module with this identifiers already exists");
        }
        Module result = await ModuleRepository.CreateModuleAsync(module);
        return Mapper.Map<ModuleResponseDTO>(result);
    }

    public async Task<ModuleResponseDTO> DeleteModuleAsync(int moduleId)
    {
        Module? module = await ModuleRepository.GetModuleByIdAsync(moduleId);
        if (module == null)
        {
            throw new Exception($"module with such id: {moduleId} does not found");
        }
        Module result = await ModuleRepository.DeleteModuleAsync(module);
        return Mapper.Map<ModuleResponseDTO>(result);
    }

    public async Task<ModuleResponseDTO> GetModuleByIdAsync(int moduleId)
    {
        Module? module = await ModuleRepository.GetModuleByIdAsync(moduleId);
        if (module == null)
        {
            throw new Exception($"module with such id: {moduleId} does not found");
        }
        return Mapper.Map<ModuleResponseDTO>(module);
    }

    public async Task<KeysetPaginationAfterResult<ModuleResponseDTO>> GetModulesKeysetPaginationAsync(string? after, string? propName, int? limit, int? moduleId, bool? reverse, int? wordsIncludeNumber)
    {
        var result = await ModuleRepository.GetModulesKeySetPaginationAsync(after, propName, limit, moduleId, reverse, wordsIncludeNumber);
        return Mapper.Map<KeysetPaginationAfterResult<ModuleResponseDTO>>(result);
    }

    public async Task<ModuleResponseDTO> UpdateModuleAsync(ModuleUpdateDTO dto)
    {
        Module module = Mapper.Map<Module>(dto);
        var result = await ModuleRepository.UpdateModuleAsync(module);
        return Mapper.Map<ModuleResponseDTO>(result);
    }
}
