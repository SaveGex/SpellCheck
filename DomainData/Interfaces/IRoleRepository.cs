
using DomainData.Models;

namespace DomainData.Interfaces;

public interface IRoleRepository
{
    Task<Role> CreateRoleAsync(Role dto);
    Task<IEnumerable<Role>> GetRolesAsync();
    Task<Role> GetRoleAsync(int roleId);
    Task<Role> UpdateRoleAsync(Role dto);
    Task<Role> DeleteRoleAsync(int roleId);
}
