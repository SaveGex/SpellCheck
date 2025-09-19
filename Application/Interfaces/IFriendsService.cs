
using Application.ModelsDTO;
using DomainData.Records;

namespace Application.Interfaces;

public interface IFriendsService
{
    Task<FriendResponseDTO> AddFriendAsync(FriendCreateDTO dto);
    Task<KeysetPaginationAfterResult<FriendResponseDTO>> GetFriendsAsync(string? after, string? propName, int? limit, int userId, bool? reverse);
    Task<FriendResponseDTO> GetFriendAsync(int friendId);
    Task<FriendResponseDTO> UpdateFriendAsync(FriendUpdateDTO dto);
    Task<FriendResponseDTO> DeleteFriendAsync(int friendId);
}
