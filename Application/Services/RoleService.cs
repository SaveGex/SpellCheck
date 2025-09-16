using Application.Interfaces;
using Application.ModelsDTO;
using AutoMapper;
using DomainData.Interfaces;
using DomainData.Models;

namespace Application.Services;

public class RoleService : IRoleService
{
    public IRoleRepository RoleRepository { get; init; }
    public IMapper Mapper { get; init; }

    public RoleService(IRoleRepository roleRepository, IMapper mapper)
    {
        RoleRepository = roleRepository;
        Mapper = mapper;
    }
    public async Task<RoleResponseDTO> CreateRoleAsync(RoleCreateDTO dto)
    {
        Role role = Mapper.Map<Role>(dto);
        RoleResponseDTO result = Mapper.Map<RoleResponseDTO>(
            await RoleRepository.CreateRoleAsync(role));
        return result;
    }

    public async Task<RoleResponseDTO> DeleteRoleAsync(int roleId)
    {
        RoleResponseDTO result = Mapper.Map<RoleResponseDTO>(
            await RoleRepository.DeleteRoleAsync(roleId));
        return result;
    }

    public async Task<RoleResponseDTO> GetRoleAsync(int roleId)
    {
        RoleResponseDTO result = Mapper.Map<RoleResponseDTO>(
            await RoleRepository.GetRoleAsync(roleId));
        return result;
    }

    public async Task<IEnumerable<RoleResponseDTO>> GetRolesAsync()
    {
        List<RoleResponseDTO> result = new List<RoleResponseDTO>();
        foreach(Role el in await RoleRepository.GetRolesAsync())
        {
            result.Add(
                Mapper.Map<RoleResponseDTO>(el));
        }
        return result;
    }

    public async Task<RoleResponseDTO> UpdateRoleAsync(RoleUpdateDTO dto)
    {
        Role role = Mapper.Map<Role>(dto);
        RoleResponseDTO result = Mapper.Map<RoleResponseDTO>(
            await RoleRepository.UpdateRoleAsync(role));
        return result;
    }
}
