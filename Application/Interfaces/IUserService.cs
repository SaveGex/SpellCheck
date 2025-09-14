using DomainData.Models;
using DomainData.Models.ModelsDTO;
using DomainData.Records;
using MR.AspNetCore.Pagination;

namespace Application.Interfaces;

public interface IUserService
{

    Task<User?> GetByEmailIncludeRolesAsync(string email);
    Task<User?> GetByPhoneIncludeRolesAsync(string phone);

    Task<UserResponseDTO> CreateUserAsync(UserCreateDTO dto);
    Task<KeysetPaginationAfterResult<UserResponseDTO>> GetUsersKeysetPaginationAsync(string? after, string? propName, int? limit, int? Id, bool? reverse);
    Task<UserResponseDTO> GetUserByIdAsync(int userId);
    Task<UserResponseDTO> UpdateUserAsync(UserUpdateDTO dto);
    Task<UserResponseDTO> DeleteUserAsync(int userId);
}
