using DbManagerApi.Services.Interfaces;
using FluentResults;
using Infrastructure;
using Infrastructure.Models;
using Infrastructure.Models.ModelsDTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DbManagerApi.Services;

public class UserService : IUserService
{
    private readonly SpellTestDbContext _context;
    public UserService(SpellTestDbContext context)
    {
        _context = context;
    }

    private static UserResponseDTO MapToDTO(User user)
    {
        return new UserResponseDTO()
        {
            Id = user.Id,
            Username = user.Username,
            //Password = user.Password,
            Number = user.Number,
            Email = user.Email,
            CreatedAt = user.CreatedAt,
            DeletedAt = user.DeletedAt
        };
    }

    /// <summary>
    ///     Returning <see cref="UserResponseDTO"/> instance if user succesfully created and throwing exception when user already exists.<br/>
    ///     By default binding role User to the user's instance
    /// </summary>
    /// <returns>
    ///     Returning <see cref="UserResponseDTO"/> wrapped into <see cref="Result"/> instance if user succesfully created<br/>
    ///     Returning <see cref="Result{UserResponseDTO}"/> which represents <b><u>Fail</u></b> state for reasons described in error messages
    /// </returns>
    public async Task<Result<UserResponseDTO>> CreateUserAsync(UserCreateDTO dto)
    {
        if (string.IsNullOrEmpty(dto.Number) && string.IsNullOrEmpty(dto.Email))
            return Result.Fail("You must provide either Number or Email.");

        bool exists = await _context.Users
            .AnyAsync(u => (dto.Number != null ? u.Number == dto.Number : false) || (dto.Email != null ? u.Email == dto.Email : false));

        if (exists)
            return Result.Fail("User already exists.");

        var newUser = new User();
        Role? userRole = await _context.Roles.SingleOrDefaultAsync(r => r.Name == RoleNames.User);
        if (userRole is null)
        {
            return Result.Fail("during attaching role process to the user occured error: userRole is null");
        }
        newUser.Roles.Add(userRole);
        _context.Entry(newUser).CurrentValues.SetValues(dto);
        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        return Result.Ok(MapToDTO(newUser));
    }

    /// <summary>
    /// Deleting user by id
    /// </summary>
    /// <param name="userId">user id</param>
    /// <returns>Result that represents operation state</returns>
    public async Task<Result<UserResponseDTO>> DeleteUserAsync(int userId)
    {
        User? user = await _context.Users.FindAsync(userId);

        if(user is null)
        {
            return Result.Fail($"User does not found.");
        }

        user.DeletedAt = DateTime.Now;
        _context.Entry(user).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return Result.Ok(MapToDTO(user));
    }

    /// <summary>
    /// Retrieves all users from the system.
    /// </summary>
    /// <returns>
    /// A <see cref="Result{T}"/> containing an <see cref="IEnumerable{UserResponseDTO}"/> 
    /// if the operation is successful; otherwise, a failure result with error details.
    /// </returns>
    public async Task<Result<IEnumerable<UserResponseDTO>>> GetUserSequenceAsync(string? propName, int? limit, int? userId, bool? reverse)
    {
        string orderBy = string.IsNullOrWhiteSpace(propName) ? nameof(User.Id) : propName!;
        int take = Math.Clamp(limit ?? 100, 1, 1000); 
        int startId = userId ?? 1;
        bool descending = reverse ?? false;

        IQueryable<User> query = _context.Users.AsQueryable()
                    .Where(u => u.Id >= startId);

        query = query.OrderByProperty(orderBy, descending);

        List<UserResponseDTO> items = await query
            .Take(take)
            .Select(u => MapToDTO(u))
            .ToListAsync();

        return Result.Ok(items.AsEnumerable());
    }


    /// <summary>
    /// Looking for user by specified <paramref name="userId"/>
    /// </summary>
    /// <param name="userId">Id of user which you want find</param>
    /// <returns><see cref="Result{UserResponseDTO}"/> which is also represents state</returns>
    public async Task<Result<UserResponseDTO>> GetUserByIdAsync(int userId)
    {
        User? user = await _context.Users.FindAsync(userId);
        if(user is null)
        {
            return Result.Fail("User does not found.");
        }

        return Result.Ok(MapToDTO(user));
    }

    /// <summary>
    /// Updating entity of user which will be finding by <paramref name="userId"/>
    /// </summary>
    /// <param name="dto">Information to update the old data of the <see cref="User"/> instance</param>
    /// <param name="userId">User id which will be updated</param>
    /// <returns><see cref="Result{UserResponseDTO}"/> that also represent operation status</returns>
    public async Task<Result<UserResponseDTO>> UpdateUserAsync(UserUpdateDTO dto, int userId)
    {
        User? user = await _context.Users.FindAsync(userId);

        if( user is null)
        {
            return Result.Fail("User does not found");
        }

        _context.Entry(user).CurrentValues.SetValues(dto);
        _context.Entry(user).State = EntityState.Modified;

        await _context.SaveChangesAsync();

        return Result.Ok(MapToDTO(user));
    }
}
