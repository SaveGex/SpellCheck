using FluentResults;
using Infrastructure.Models.ModelsDTO;

namespace DbManagerApi.Services.Interfaces
{
    public interface IGetAllEntitiesAsync<TResponse>
    {
        Task<Result<IEnumerable<TResponse>>> GetAllEntitiesAsync();

    }
}
