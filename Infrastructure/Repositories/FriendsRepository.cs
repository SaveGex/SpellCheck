using DomainData.Interfaces;
using DomainData.Models;
using DomainData.Records;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;
using MR.AspNetCore.Pagination;
using MR.EntityFrameworkCore.KeysetPagination;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class FriendsRepository
    : IFriendsRepository
{
    SpellTestDbContext Context { get; init; }
    IPaginationService PaginationService { get; init; }

    public FriendsRepository(SpellTestDbContext context, IPaginationService paginationService)
    {
        Context = context;
        PaginationService = paginationService;
    }

    public Task<string> GetCursorBase64StringAsync(Friend? cursorFriend)
    {
        return Task.FromResult(cursorFriend is not null ?
            cursorFriend.Id.ToString() :
            "No more content...");
    }


    public async Task<Friend> AddFriendAsync(Friend friend)
    {
        var result = Context.Friends.Add(friend).Entity;
        await Context.SaveChangesAsync();
        return result;
    }

    public async Task<Friend> DeleteFriendAsync(Friend friend)
    {
        var result = Context.Friends.Remove(friend).Entity;
        await Context.SaveChangesAsync();
        return result;
    }

    public async Task<Friend> GetFriendAsync(int friendId)
    {
        Friend? friend = await Context.Friends.FindAsync(friendId);
        if (friend == null)
        {
            throw new Exception("Friend with this Id does not found");
        }
        return friend;
    }

    public async Task<KeysetPaginationAfterResult<Friend>> GetFriendsAsync(string? after, string? propName, int? limit, int userId, bool? reverse)
    {
        {
            int afterInt;
            if (int.TryParse(after, out afterInt) && afterInt > await Context.Friends.MaxAsync(m => m.Id))
            {
                int MaxId = await Context.Friends.MaxAsync(m => m.Id);
                after = MaxId.ToString();
            }
        }
        IQueryable<Friend> query = Context.Friends;

        query = query
            .Where(f => f.ToIndividualId == userId || f.FromIndividualId == userId);


        KeysetQueryModel queryModel = new KeysetQueryModel()
        {
            Size = limit ?? 20,
            After = after,
        };

        Action<KeysetPaginationBuilder<Friend>> actionKeysetPaginationBuilder;
        try
        {
            actionKeysetPaginationBuilder = CreateActionKeysetPaginationBuilder(propName, reverse);
        }
        catch
        {
            actionKeysetPaginationBuilder = CreateActionKeysetPaginationBuilder(nameof(Friend.Id), reverse);
        }

        KeysetPaginationResult<Friend> result = await PaginationService.KeysetPaginateAsync(
            query,
            CreateActionKeysetPaginationBuilder(propName, reverse),
            async id => await Context.Friends.FindAsync(int.Parse(id)),
            queryModel: queryModel
        );

        return new KeysetPaginationAfterResult<Friend>(
            await GetCursorBase64StringAsync(
                result.Data.LastOrDefault()),
            result);
    }

    public async Task<Friend> UpdateFriendAsync(Friend friend)
    {
        var result = Context.Friends.Update(friend).Entity;
        await Context.SaveChangesAsync();
        return result;
    }

    private Action<KeysetPaginationBuilder<Friend>> CreateActionKeysetPaginationBuilder(string? propName, bool? reverse)
    {
        var parameter = Expression.Parameter(typeof(Friend), "m");
        var property = Expression.PropertyOrField(parameter, propName ?? nameof(Friend.Id));
        var propertyType = property.Type;

        var lambdaType = typeof(Func<,>).MakeGenericType(typeof(Friend), propertyType);
        var lambda = Expression.Lambda(lambdaType, property, parameter);

        return builder =>
        {
            var ascendingMethod = typeof(KeysetPaginationBuilder<Friend>)
                .GetMethods()
                .First(m => m.Name == "Ascending" && m.IsGenericMethod)
                .MakeGenericMethod(propertyType);

            var descendingMethod = typeof(KeysetPaginationBuilder<Friend>)
                .GetMethods()
                .First(m => m.Name == "Descending" && m.IsGenericMethod)
                .MakeGenericMethod(propertyType);

            var propertyId = Expression.Lambda<Func<Friend, int>>(
                Expression.PropertyOrField(parameter, nameof(Friend.Id)),
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
