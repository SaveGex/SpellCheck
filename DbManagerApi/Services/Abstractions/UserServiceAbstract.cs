using DbManagerApi.Services.Interfaces;
using FluentResults;
using Infrastructure.Models.ModelsDTO;

namespace DbManagerApi.Services.Abstractions
{
    public abstract class UserServiceAbstract :
        IGetEntitiesSequenceAsync<UserResponseDTO>,
        ICreateEntityAsync<UserResponseDTO, UserCreateDTO>,
        IDeleteEntityAsync<UserResponseDTO>,
        IGetEntityByIdAsync<UserResponseDTO>,
        IUpdateEntityAsync<UserResponseDTO, UserUpdateDTO>
    {
        public abstract Task<Result<UserResponseDTO>> CreateEntityAsync(UserCreateDTO dto);
        public abstract Task<Result<UserResponseDTO>> DeleteEntityAsync(int moduleId);
        public abstract Task<Result<IEnumerable<UserResponseDTO>>> GetEntitiesSequenceAsync(string? propName, int? limit, int? moduleId, bool? reverse);
        public abstract Task<Result<UserResponseDTO>> GetEntityByIdAsync(int moduleId);
        public abstract Task<Result<UserResponseDTO>> UpdateEntityAsync(UserUpdateDTO dto, int moduleId);
    }
}
