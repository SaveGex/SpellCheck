using FluentResults;

namespace DbManagerApi.Services.Interfaces
{
    public interface IGetAllEntitiesAsync<TResponse>
    {
        Task<Result<IEnumerable<TResponse>>> GetAllEntitiesAsync();

    }
}
