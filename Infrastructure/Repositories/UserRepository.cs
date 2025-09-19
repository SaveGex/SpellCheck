using DomainData;
using DomainData.Models;
using Microsoft.EntityFrameworkCore;
using MR.AspNetCore.Pagination;
using MR.EntityFrameworkCore.KeysetPagination;
using System.Linq.Expressions;
using DomainData.Interfaces;
using DomainData.Records;

namespace DbManagerApi.Services.UserServices;

public sealed class UserRepository : IUserRepository
{
    private readonly SpellTestDbContext _context;
    
    private IPaginationService _paginationService;

    public UserRepository(SpellTestDbContext context, IPaginationService paginationService)
    {
        _context = context;
        _paginationService = paginationService;
    }


    public async Task<User> RemoveRoleToUserAsync(int userId, int roleId)
    {
        User? user = await _context.Users.FindAsync(userId);
        if (user == null)
        {
            throw new Exception("User does not found by this Id");
        }

        Role? role = await _context.Roles.FindAsync(roleId);
        if (role == null)
        {
            throw new Exception("Role does not found by this id");
        }

        if (user.Roles.Contains(role))
        {
            throw new Exception($"User already have {role.Name} role");
        }

        user.Roles.Add(role);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User> RemoveRoleFromUserAsync(int userId, int roleId)
    {
        User? user = await _context.Users.FindAsync(userId);
        if (user == null)
        {
            throw new Exception("User does not found by this Id");
        }

        Role? role = await _context.Roles.FindAsync(roleId);
        if (role == null)
        {
            throw new Exception("Role does not found by this id");
        }

        if (user.Roles.Contains(role))
        {
            throw new Exception($"User already have {role.Name} role");
        }

        user.Roles.Remove(role);
        await _context.SaveChangesAsync();
        return user;
    }


    public async Task<User?> GetByEmailIncludeRolesAsync(string email)
    {
        return await _context.Users
            .Include(u => u.Roles)
            .SingleOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetByPhoneIncludeRolesAsync(string phone)
    {
        return await _context.Users
            .Include(u => u.Roles)
            .SingleOrDefaultAsync(u => u.Number == phone);
    }


    public Task<string> GetCursorBase64StringAsync(User? cursorUser)
    {
        return Task.FromResult(
            cursorUser?.Id.ToString() ?? "No more content..."
            );
    }


    public async Task<bool> ExistsAsync(string? number, string? email)
    {
        return await _context.Users
            .AnyAsync(u => (number != null ? u.Number == number : false)
                        || (email != null ? u.Email == email : false));
    }
    
    
    public async Task<KeysetPaginationAfterResult<User>> GetUsersKeysetPaginationAsync(string? after, string? propName, int? limit, int? Id, bool? reverse)
    {
        {
            int afterInt;
            if (int.TryParse(after, out afterInt) && afterInt > await _context.Users.MaxAsync(m => m.Id))
            {
                int MaxId = await _context.Users.MaxAsync(m => m.Id);
                after = MaxId.ToString();
            }
        }
        IQueryable<User> query = _context.Users;

        if (Id is not null)
        {
            query = query
                .Include(u => u.Roles)
                .Where(m => m.Id == Id);
        }

        KeysetQueryModel queryModel = new KeysetQueryModel()
        {

            Size = limit ?? 20,
            After = after,
        };

        Action<KeysetPaginationBuilder<User>> actionKeysetPaginationBuilder;
        try
        {
            actionKeysetPaginationBuilder = CreateActionKeysetPaginationBuilder(propName, reverse);
        }
        catch
        {
            actionKeysetPaginationBuilder = CreateActionKeysetPaginationBuilder(nameof(User.Id), reverse);
        }

        KeysetPaginationResult<User> result = await _paginationService.KeysetPaginateAsync(
            query,
            CreateActionKeysetPaginationBuilder(propName, reverse),
            async id => await _context.Users.FindAsync(int.Parse(id)),
            queryModel: queryModel
        );

        return new KeysetPaginationAfterResult<User>(
            await GetCursorBase64StringAsync(
                result.Data.LastOrDefault()), 
            result);
    }

    public async Task<User> CreateUserAsync(User user, RoleNames? roleArgument = null)
    {
        RoleNames role;
        switch (roleArgument)
        {
            case null: role = RoleNames.User; break;
            case not null: role = roleArgument.Value; break;
        }

        user = await AttachRoleAsync(user, role);
        
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }


    public async Task<User> DeleteUserAsync(User user)
    {
        user.DeletedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return user;
    }


    public async Task<User> GetUserByIdAsync(int userId)
    {
        User? user = await _context.Users
            .Include(u => u.Roles)
            .Where(u => u.Id  == userId)
            .SingleOrDefaultAsync();
        if (user is null)
        {
            throw new Exception("User does not found by this Id...");
        }

        return user;
    }


    public async Task<User> UpdateUserAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        return user;
    }


    private async Task<User> AttachRoleAsync(User user, RoleNames roleName)
    {
        Role? baseRole = await _context.Roles
            .Where(r => r.Name == roleName.ToString())
            .FirstOrDefaultAsync();

        if (baseRole is null)
        {
            throw new Exception("base role for attach to the user does not found");
        }
        user.Roles.Add(baseRole);
        return user;
    }

    private Action<KeysetPaginationBuilder<User>> CreateActionKeysetPaginationBuilder(string? propName, bool? reverse)
    {
        var parameter = Expression.Parameter(typeof(User), "m");
        var property = Expression.PropertyOrField(parameter, propName ?? nameof(User.Id));
        var propertyType = property.Type;

        var lambdaType = typeof(Func<,>).MakeGenericType(typeof(User), propertyType);
        var lambda = Expression.Lambda(lambdaType, property, parameter);

        return builder =>
        {
            var ascendingMethod = typeof(KeysetPaginationBuilder<User>)
                .GetMethods()
                .First(m => m.Name == "Ascending" && m.IsGenericMethod)
                .MakeGenericMethod(propertyType);

            var descendingMethod = typeof(KeysetPaginationBuilder<User>)
                .GetMethods()
                .First(m => m.Name == "Descending" && m.IsGenericMethod)
                .MakeGenericMethod(propertyType);

            var propertyId = Expression.Lambda<Func<User, int>>(
                Expression.PropertyOrField(parameter, nameof(User.Id)),
                parameter
            );

            if (reverse == true)
            {
                descendingMethod.Invoke(builder, new object[] { lambda });
                builder.Descending(propertyId);
            }
            else
            {
                ascendingMethod.Invoke(builder, new object[] { lambda });
                builder.Ascending(propertyId);
            }
        };
    }
}
