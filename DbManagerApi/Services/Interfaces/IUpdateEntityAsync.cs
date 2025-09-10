using FluentResults;

namespace DbManagerApi.Services.Interfaces
{
    public interface IUpdateEntityAsync<TResponse, TUpdate>
    {
        Task<Result<TResponse>> UpdateEntityAsync(TUpdate dto, int moduleId);

    }
}
