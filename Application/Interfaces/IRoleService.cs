
using Application.ModelsDTO;

namespace Application.Interfaces;
public interface IRoleService
{
    Task<RoleResponseDTO> CreateRoleAsync(RoleCreateDTO dto);
    Task<IEnumerable<RoleResponseDTO>> GetRolesAsync();
    Task<RoleResponseDTO> GetRoleAsync(int roleId);
    Task<RoleResponseDTO> UpdateRoleAsync(RoleUpdateDTO dto);
    Task<RoleResponseDTO> DeleteRoleAsync(int roleId);
}
