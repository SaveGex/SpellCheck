using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure;
using Infrastructure.Models;
using System.Diagnostics;
using DbManagerApi.Models_DTO;
using System.Text.Json;

namespace DbManagerApi.Controllers
{
    [Route("api/Users")]
    [ApiController]
    public class FriendsController : ControllerBase
    {
        private readonly SpellTestDbContext _context;

        public FriendsController(SpellTestDbContext context)
        {
            _context = context;
        }

        // GET: api/Users/{userId}/Friends
        // GET: api/Users/Friends
        [Route("{userId:int}/[controller]")]
        [Route("[controller]")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FriendDTO>>> GetFriends(int? userId)
        {
            if (userId is not null)
            {
                return await _context.Friends.Where(f => f.FromIndividualId == userId || f.ToIndividualId == userId)
                    .Select(f => new FriendDTO(f))
                    .ToListAsync();
            }
            return await _context.Friends
                .Select(f => new FriendDTO(f))
                .ToListAsync();
        }

        // POST: api/Users/{userId:int}/Friends/{friendId:int}
        [Route("{userId:int}/[controller]/{friendId:int}")]
        [HttpPost]
        public async Task<IResult> PostFriend(int userId, int friendId)
        {
            if (userId == friendId)
            {
                return await Task.FromResult<IResult>(TypedResults.BadRequest("User cannot be friend with themselves."));
            }

            int from_individual_id, to_individual_id;
            (from_individual_id, to_individual_id) = (userId, friendId) switch
            {
                (var a, var b) when a < b => (a, b),
                _ => (friendId, userId)
            };

            User? user = await _context.Users.FirstOrDefaultAsync(u => u.Id == from_individual_id);
            User? userFriend = await _context.Users.FirstOrDefaultAsync(u => u.Id == to_individual_id);

            if (user is null || userFriend is null)
            {
                return await Task.FromResult<IResult>(TypedResults.NotFound("User or friend not found."));
            }

            Friend friend = new()
            {
                FromIndividualId = user.Id,
                ToIndividualId = userFriend.Id,
                FromIndividual = user,
                ToIndividual = userFriend
            };
            if (await _context.Friends.AnyAsync(f => f.FromIndividualId == friend.FromIndividualId && f.ToIndividualId == friend.ToIndividualId))
            {
                return await Task.FromResult<IResult>(TypedResults.BadRequest("Friendship already exists."));
            }
            try
            {
                _context.Friends.Add(friend);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return await Task.FromResult<IResult>(TypedResults.BadRequest("Failed to add friend." + ex.Message));
            }
            return await Task.FromResult<IResult>(TypedResults.Created(JsonSerializer.Serialize(new FriendDTO(friend))));
        }

        // DELETE: api/Users/{userId:int}/Friends/{friendId:int}
        [Route("{userId:int}/[controller]/{friendId:int}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteFriend(int userId, int friendId)
        {
            int from_individual_id, to_individual_id;
            (from_individual_id, to_individual_id) = (userId, friendId) switch
            {
                (var a, var b) when a < b => (a, b),
                _ => (friendId, userId)
            };
            Friend? friend = await _context.Friends.FirstOrDefaultAsync(f => f.FromIndividualId == from_individual_id && f.ToIndividualId == to_individual_id);
            if (friend is null)
            {
                return NotFound();
            }

            _context.Friends.Remove(friend);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
