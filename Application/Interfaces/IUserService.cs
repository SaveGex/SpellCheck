using Application.ModelsDTO;
using DomainData.Models;
using DomainData.Records;

namespace Application.Interfaces;

public interface IUserService
{
    Task<UserResponseDTO> AddRoleToUserAsync(int userId, int roleId);
    Task<UserResponseDTO> RemoveRoleFromUserAsync(int userId, int roleId);

    Task<User?> GetByEmailIncludeRolesAsync(string email);
    Task<User?> GetByPhoneIncludeRolesAsync(string phone);

    Task<KeysetPaginationAfterResult<UserResponseDTO>> GetUsersKeysetPaginationAsync(string? after, string? propName, int? limit, int? Id, bool? reverse);
    Task<UserResponseDTO> GetUserByIdAsync(int userId);
    Task<UserResponseDTO> UpdateUserAsync(UserUpdateDTO dto);
    Task<UserResponseDTO> DeleteUserAsync(int userId);

}
