using FluentResults;
using Infrastructure.Models.ModelsDTO;

namespace DbManagerApi.Services.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TResponse">Entity response type</typeparam>
    public interface IGetEntityByIdAsync<TResponse>
    {
        Task<Result<TResponse>> GetEntityByIdAsync(int moduleId);
    }
}
