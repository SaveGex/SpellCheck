using FluentResults;
using Infrastructure.Models;
using Infrastructure.Models.ModelsDTO;

namespace DbManagerApi.Services.Interfaces;

public interface IUserService
{
    Task<Result<IEnumerable<UserResponseDTO>>> GetAllUsersAsync();
    Task<Result<UserResponseDTO>> GetUserByIdAsync(int userId);
    Task<Result<UserResponseDTO>> CreateUserAsync(UserCreateDTO dto);
    Task<Result<UserResponseDTO>> UpdateUserAsync(UserUpdateDTO dto, int userId);
    Task<Result<UserResponseDTO>> DeleteUserAsync(int userId);
    //Task AddRoleToUserAsync(int userId, int roleId);
    //Task RemoveRoleFromUserAsync(int userId, int roleId);
}
