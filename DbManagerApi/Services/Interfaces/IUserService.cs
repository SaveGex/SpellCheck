using FluentResults;
using Infrastructure.Models;
using Infrastructure.Models.ModelsDTO;

namespace DbManagerApi.Services.Interfaces;

public interface IUserService
{
    /// <summary>
    /// Returning an array of <see cref="UserResponseDTO"/>
    /// </summary>
    /// <param name="userId">entry point of sequence</param>
    /// <param name="orderByPropName">property name of class by which a table be sorted</param>
    /// <param name="limit">how many entities maximum can be in the sequence</param>
    /// <returns>the sequence of <see cref="UserResponseDTO"/></returns>
    Task<Result<IEnumerable<UserResponseDTO>>> GetUserSequenceAsync(string? propName, int? limit, int? userId, bool? reverse);
    Task<Result<UserResponseDTO>> GetUserByIdAsync(int userId);
    Task<Result<UserResponseDTO>> CreateUserAsync(UserCreateDTO dto);
    Task<Result<UserResponseDTO>> UpdateUserAsync(UserUpdateDTO dto, int userId);
    Task<Result<UserResponseDTO>> DeleteUserAsync(int userId);
    //Task AddRoleToUserAsync(int userId, int roleId);
    //Task RemoveRoleFromUserAsync(int userId, int roleId);
}
