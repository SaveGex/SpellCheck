using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure;
using Infrastructure.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch.Exceptions;

namespace DbManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly SpellTestDbContext _context;

        public UsersController(SpellTestDbContext context)
        {
            _context = context;
        }
        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            return await _context.Users.Select(u => new UserDTO(u)).ToListAsync();
        }
        // GET: api/Users/{userId:int}
        [HttpGet("{userId:int}")]
        public async Task<ActionResult<UserDTO>> GetUser(int userId)
        {
            if (await _context.Users.FirstOrDefaultAsync(u => u.Id == userId) is User foundUser)
                return new UserDTO(foundUser);

            return NotFound();            
        }
        /// <summary>
        ///     PATCH api/Users/{userId:int}
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userPatch"></param>
        /// <returns></returns>
        /// <exception cref="BadRequest">If user with specified Id not found</exception>
        [HttpPatch("{userId:int}")]
        public async Task<IResult> PatchUser(int userId, [FromBody]JsonPatchDocument<User> userPatch)
        {
            if(userPatch is null)
            {
                return await Task.FromResult<IResult>(TypedResults.BadRequest("User patch is null"));
            }
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user is null)
            {
                return await Task.FromResult<IResult>(TypedResults.NotFound("User not found"));
            }
            try
            {
                userPatch.ApplyTo(user, ModelState);
            }
            catch (ArgumentNullException ex)
            {
                return await Task.FromResult<IResult>(TypedResults.BadRequest($"Invalid patch document: {ex.Message}"));
            }

            if (!ModelState.IsValid)
            {
                return await Task.FromResult<IResult>(TypedResults.BadRequest(ModelState));
            }

            _context.Entry(user).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return await Task.FromResult<IResult>(TypedResults.Ok(new UserDTO(user)));
            }
            catch (DbUpdateException)
            {
                return await Task.FromResult<IResult>(TypedResults.InternalServerError("An error occurred while updating the user."));
            }
        }
        // PUT: api/Users/5
        [HttpPut("{userId:int}")]
        public async Task<IActionResult> PutUser(UserDTO user, int userId)
        {
            User? existsUser = await _context.Users.FindAsync(userId);

            if(existsUser is null)
            {
                return NotFound("Incorrect Id. User not found");
            }

            existsUser.Username = user.Username;
            existsUser.Password = user.Password;
            existsUser.Email = user.Email;
            existsUser.Number = user.Number;


            await _context.SaveChangesAsync();

            return NoContent();
        }
        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<UserDTO>> PostUser(User user)
        {
            if(string.IsNullOrEmpty(user.Email) && string.IsNullOrEmpty(user.Number))
            {
                return BadRequest("Must specify unique fields: Email or Number");
            }

            User? existsUser = await FindUserAsync(user);
            if (existsUser is not null)
            {
                string existed_field = (existsUser, user) switch
                {
                    var (u1, u2) when u1.Number == u2.Number => "number",
                    var (u1, u2) when u1.Email == u2.Email => "email",
                    _ => "something went wrong"
                };
                return BadRequest($"User with this \"{existed_field}\" already exists");
            }

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            UserDTO userDTO = new(user);
            return CreatedAtAction("GetUser", userDTO.Id, userDTO);
        }
        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
        /// <summary>
        /// Looks for an existing user by Key properties: Id, Number, Email.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>null when user doesn't exists in DB. Otherwise return User instance</returns>
        private async Task<User?> FindUserAsync(User user)
        {
            switch (user)
            {
                case { Id: not 0 }:
                    return await _context.Users.FindAsync(user.Id);
                case { Number: not null }:
                    return await _context.Users.FirstOrDefaultAsync(u => u.Number == user.Number);
                case { Email: not null }:
                    return await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
                default:
                    return null;
            }
        
        }
    }
}
