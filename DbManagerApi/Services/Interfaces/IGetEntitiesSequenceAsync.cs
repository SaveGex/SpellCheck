using FluentResults;

namespace DbManagerApi.Services.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TResponse">Entity response type</typeparam>
    public interface IGetEntitiesSequenceAsync<TResponse>
    {
        Task<Result<IEnumerable<TResponse>>> GetEntitiesSequenceAsync(string? propName, int? limit, int? moduleId, bool? reverse);
    }
}
