using FluentResults;
using Infrastructure.Models.ModelsDTO;

namespace DbManagerApi.Services.Interfaces
{
    public interface IDeleteEntityAsync<TResponse>
    {
        Task<Result<TResponse>> DeleteEntityAsync(int moduleId);

    }
}
