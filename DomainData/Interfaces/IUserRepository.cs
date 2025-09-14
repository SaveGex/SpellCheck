using DomainData.Models;
using DomainData.Records;

namespace DomainData.Interfaces
{
    public interface IUserRepository
    {

        Task<User?> GetByEmailIncludeRolesAsync(string email);
        Task<User?> GetByPhoneIncludeRolesAsync(string phone);

        Task<string> GetCursorBase64StringAsync(User? cursorUser);
        Task<bool> ExistsAsync(string? number, string? email);

        Task<KeysetPaginationAfterResult<User>> GetUsersKeysetPaginationAsync(string? after, string? propName, int? limit, int? Id, bool? reverse);
        Task<User> CreateUserAsync(User user, RoleNames? roleArgument = null);
        Task<User> DeleteUserAsync(User user);
        Task<User> GetUserByIdAsync(int userId);
        Task<User> UpdateUserAsync(User user);

    }
}
