using Application.Interfaces;
using Application.ModelsDTO;
using DbManagerApi.Controllers.Filters.FilterAttributes;
using DomainData.Models;
using DomainData.Records;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;

namespace DbManagerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [RestrictOfCreateFriendships]
    public class FriendsController : ControllerBase
    {
        IFriendsService FriendsService { get; init; }

        public FriendsController(IFriendsService friendsService)
        {
            FriendsService = friendsService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(FriendResponseDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<FriendResponseDTO>> AddFriend(
            [FromBody] FriendCreateDTO dto)
        {
            FriendResponseDTO result;
            try
            {
                result = await FriendsService.AddFriendAsync(dto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(result);
        }


        /// <summary>
        /// users can gets only their own friends
        /// userId auto-binding by <see cref="BidningUserIdFilterAttribute"/> for this goal
        /// </summary>
        /// <param name="after"></param>
        /// <param name="propName"></param>
        /// <param name="limit"></param>
        /// <param name="reverse"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(KeysetPaginationAfterResult<FriendResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<KeysetPaginationAfterResult<FriendResponseDTO>>> GetFriends(
            [FromQuery] string? after,
            [FromQuery] string? propName,
            [FromQuery] int? limit,
            [FromQuery] bool? reverse)
        {
            int userId = int.Parse(HttpContext.User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
            KeysetPaginationAfterResult<FriendResponseDTO> result;
            try
            {
                result = await FriendsService.GetFriendsAsync(after, propName, limit, userId, reverse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(result);
        }


        [HttpGet("{friendId:int}")]
        [ProducesResponseType(typeof(FriendResponseDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<KeysetPaginationAfterResult<FriendResponseDTO>>> GetFriend(
            int friendId)
        {
            FriendResponseDTO result;
            try
            {
                result = await FriendsService.GetFriendAsync(friendId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(result);
        }

        [HttpPut]
        [ProducesResponseType(typeof(FriendResponseDTO), StatusCodes.Status200OK)]
        [UserOwnership("friendId", $"{nameof(Friend)}s")]
        public async Task<ActionResult<FriendResponseDTO>> UpdateFriend(
            [FromBody] FriendUpdateDTO dto)
        {
            FriendResponseDTO result;
            try
            {
                result = await FriendsService.UpdateFriendAsync(dto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(result);
        }


        [HttpDelete("{friendId:int}")]
        [ProducesResponseType(typeof(FriendResponseDTO), StatusCodes.Status200OK)]
        [UserOwnership("friendId", $"{nameof(Friend)}s")]
        public async Task<ActionResult<FriendResponseDTO>> DeleteFriend(
            int friendId)
        {
            FriendResponseDTO result;
            try
            {
                result = await FriendsService.DeleteFriendAsync(friendId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(result);
        }

    }
}
