using Infrastructure.Models;

namespace DbManagerApi.Models_DTO;

public class FriendDTO
{
    public int Id { get; set; }

    public int FromIndividualId { get; set; }

    public int ToIndividualId { get; set; }

    public FriendDTO(Friend friend)
    {
        this.Id = friend.Id;
        this.FromIndividualId = friend.FromIndividualId;
        this.ToIndividualId = friend.ToIndividualId;
    }
}
