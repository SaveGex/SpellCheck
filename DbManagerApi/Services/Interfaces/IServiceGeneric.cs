using FluentResults;

namespace DbManagerApi.Services.Interfaces;

public interface IServiceGeneric<SourceType, ResponseType, UpdateType, CreateType>
{
    Task<Result<IEnumerable<ResponseType>>> GetAllAsync();
    Task<Result<ResponseType>> GetByIdAsync(int id);
    Task<Result<ResponseType>> CreateAsync(CreateType dto);
    Task<Result<ResponseType>> UpdateAsync(UpdateType dto, int id);
    Task<Result<ResponseType>> DeleteAsync(int id);
}
