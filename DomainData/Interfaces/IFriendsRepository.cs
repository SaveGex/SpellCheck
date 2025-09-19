using DomainData.Models;
using DomainData.Records;

namespace DomainData.Interfaces;

public interface IFriendsRepository
{
    Task<string> GetCursorBase64StringAsync(Friend? cursorFriend);

    Task<Friend> AddFriendAsync(Friend friend);
    Task<KeysetPaginationAfterResult<Friend>> GetFriendsAsync(string? after, string? propName, int? limit, int userId, bool? reverse);
    Task<Friend> GetFriendAsync(int friendId);
    Task<Friend> UpdateFriendAsync(Friend friend);
    Task<Friend> DeleteFriendAsync(Friend friend);
}
