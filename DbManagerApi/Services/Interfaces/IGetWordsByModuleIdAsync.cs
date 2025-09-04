using FluentResults;

namespace DbManagerApi.Services.Interfaces
{
    public interface IGetWordsByModuleIdAsync<TWords>
    {
        Task<Result<IEnumerable<TWords>>> GetWordsByModuleIdAsync(int moduleId);

    }
}
