
using Application.Interfaces;
using Application.ModelsDTO;
using AutoMapper;
using DomainData.Interfaces;
using DomainData.Models;
using DomainData.Records;

namespace Application.Services;

public class UserService : IUserService
{

    private IUserRepository UserRepository { get; init; }

    private IMapper Mapper { get; init; }


    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        UserRepository = userRepository;
        Mapper = mapper;
    }


    public async Task<UserResponseDTO> AddRoleToUserAsync(int userId, int roleId)
    {
        User result = await UserRepository.AttachRoleToUserAsync(userId, roleId);
        return Mapper.Map<UserResponseDTO>(result);
    }

    public async Task<UserResponseDTO> RemoveRoleFromUserAsync(int userId, int roleId)
    {
        User result = await UserRepository.AttachRoleToUserAsync(userId, roleId);
        return Mapper.Map<UserResponseDTO>(result);
    }


    public async Task<User?> GetByEmailIncludeRolesAsync(string email)
    {
        return await UserRepository.GetByEmailIncludeRolesAsync(email);
    }


    public async Task<User?> GetByPhoneIncludeRolesAsync(string phone)
    {
        return await UserRepository.GetByPhoneIncludeRolesAsync(phone);
    }


    public async Task<UserResponseDTO> DeleteUserAsync(int userId)
    {
        User? user = await UserRepository.GetUserByIdAsync(userId);
        if (user is null)
        {
            throw new Exception($"user by id {userId} does not found.");
        }
        UserResponseDTO dto = Mapper.Map<UserResponseDTO>(
            await UserRepository.DeleteUserAsync(user));
        return dto;
    }


    public async Task<UserResponseDTO> GetUserByIdAsync(int userId)
    {
        User? user = await UserRepository.GetUserByIdAsync(userId);
        if (user is null)
        {
            throw new Exception($"user by id {userId} does not found.");
        }
        UserResponseDTO dto = Mapper.Map<UserResponseDTO>(user);
        return dto;
    }


    public async Task<KeysetPaginationAfterResult<UserResponseDTO>> GetUsersKeysetPaginationAsync(string? after, string? propName, int? limit, int? Id, bool? reverse)
    {
        var result = await UserRepository.GetUsersKeysetPaginationAsync(after, propName, limit, Id, reverse);
        var resultDTO = Mapper.Map<KeysetPaginationAfterResult<UserResponseDTO>>(result);
        return resultDTO;
    }


    public async Task<UserResponseDTO> UpdateUserAsync(UserUpdateDTO dto)
    {
        if (await UserRepository.ExistsAsync(dto.Number, dto.Email))
        {
            throw new Exception("User with such email or number does not found");
        }

        if (dto.Password.Length < 4)
        {
            throw new Exception("Password must be at least 4 characters long.");
        }

        User? user = Mapper.Map<User>(dto);
        User result = await UserRepository.UpdateUserAsync(user);
        return Mapper.Map<UserResponseDTO>(result);
    }
}
