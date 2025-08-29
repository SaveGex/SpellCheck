using DbManagerApi.Services.Interfaces;
using FluentResults;
using Infrastructure.Models.ModelsDTO;

namespace DbManagerApi.Services.Abstractions
{
    public abstract class WordServiceAbstraction :
        IGetEntitiesSequenceAsync<WordResponseDTO>,
        ICreateEntityAsync<WordResponseDTO, WordCreateDTO>,
        IDeleteEntityAsync<WordResponseDTO>,
        IGetEntityByIdAsync<WordResponseDTO>,
        IUpdateEntityAsync<WordResponseDTO, WordUpdateDTO>,
        IGetAllEntitiesAsync<WordResponseDTO>,
        IGetWordsByModuleIdAsync<WordResponseDTO>
    {
        public abstract Task<Result<WordResponseDTO>> CreateEntityAsync(WordCreateDTO dto);
        public abstract Task<Result<WordResponseDTO>> DeleteEntityAsync(int moduleId);
        public abstract Task<Result<IEnumerable<WordResponseDTO>>> GetAllEntitiesAsync();
        public abstract Task<Result<IEnumerable<WordResponseDTO>>> GetEntitiesSequenceAsync(string? propName, int? limit, int? moduleId, bool? reverse);
        public abstract Task<Result<WordResponseDTO>> GetEntityByIdAsync(int moduleId);
        public abstract Task<Result<IEnumerable<WordResponseDTO>>> GetWordsByModuleIdAsync(int moduleId);
        public abstract Task<Result<WordResponseDTO>> UpdateEntityAsync(WordUpdateDTO dto, int moduleId);
    }
}
