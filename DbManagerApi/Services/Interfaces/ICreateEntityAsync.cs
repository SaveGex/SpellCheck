using FluentResults;
using Infrastructure.Models.ModelsDTO;

namespace DbManagerApi.Services.Interfaces
{
    public interface ICreateEntityAsync<TResponse, TCreate>
    {
        Task<Result<TResponse>> CreateEntityAsync(TCreate dto);
    }
}
