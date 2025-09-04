using FluentResults;

namespace DbManagerApi.Services.Interfaces
{
    public interface IDeleteEntityAsync<TResponse>
    {
        Task<Result<TResponse>> DeleteEntityAsync(int moduleId);

    }
}
