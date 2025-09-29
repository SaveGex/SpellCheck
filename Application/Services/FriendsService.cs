using Application.Interfaces;
using Application.ModelsDTO;
using AutoMapper;
using DomainData.Interfaces;
using DomainData.Models;
using DomainData.Records;

namespace Application.Services;

public class FriendsService
    : IFriendsService
{
    IFriendsRepository FriendsRepository { get; init; }
    IUserRepository UserRepository { get; init; }
    IMapper Mapper { get; init; }

    public FriendsService(IFriendsRepository friendsRepository, IUserRepository userRepository, IMapper mapper)
    {
        FriendsRepository = friendsRepository;
        UserRepository = userRepository;
        Mapper = mapper;
    }


    public async Task<FriendResponseDTO> AddFriendAsync(FriendCreateDTO dto)
    {

        await UserRepository.GetUserByIdAsync(dto.ToIndividualId);
        await UserRepository.GetUserByIdAsync(dto.FromIndividualId);


        if (dto.ToIndividualId == dto.FromIndividualId)
        {
            throw new Exception("User cannot be a friend with himself");
        }
        Friend friend = Mapper.Map<Friend>(dto);
        var result = await FriendsRepository.AddFriendAsync(friend);
        return Mapper.Map<FriendResponseDTO>(
            result);
    }

    public async Task<FriendResponseDTO> DeleteFriendAsync(int friendId)
    {
        Friend friendToDelete = await FriendsRepository.GetFriendAsync(friendId);
        var result = await FriendsRepository.DeleteFriendAsync(friendToDelete);
        return Mapper.Map<FriendResponseDTO>(
            result);
    }

    public async Task<FriendResponseDTO> GetFriendAsync(int friendId)
    {
        Friend result = await FriendsRepository.GetFriendAsync(friendId);
        return Mapper.Map<FriendResponseDTO>(
            result);
    }

    public async Task<KeysetPaginationAfterResult<FriendResponseDTO>> GetFriendsAsync(string? after, string? propName, int? limit, int userId, bool? reverse)
    {
        var result = await FriendsRepository.GetFriendsAsync(after, propName, limit, userId, reverse);
        return Mapper.Map<KeysetPaginationAfterResult<FriendResponseDTO>>(
            result);
    }

    public async Task<FriendResponseDTO> UpdateFriendAsync(FriendUpdateDTO dto)
    {
        if (dto.ToIndividualId == dto.FromIndividualId)
        {
            throw new Exception("User cannot be a friend with himself");
        }
        Friend friendToUpdate = Mapper.Map<Friend>(dto);
        var result = await FriendsRepository.UpdateFriendAsync(friendToUpdate);
        return Mapper.Map<FriendResponseDTO>(
            result);
    }
}
