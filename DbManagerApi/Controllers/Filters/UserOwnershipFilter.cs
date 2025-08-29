using DbManagerApi.Services.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace DbManagerApi.Controllers.Filters;

public class UserOwnershipFilter : IAsyncAuthorizationFilter
{
    private readonly IEntityOwnershipService _ownershipService;
    private readonly string _entityIdKey;
    private readonly string _entityName;

    /// <summary>
    /// Specifies that a user ownership of an entity or it's have special permission (admin, manager)
    /// </summary>
    /// <param name="entityIdKey">The key representing a <see cref="int"/> variable from route which is contains Id of an entity</param>
    /// <param name="entityName">The name in plural of the entity to which the filter applies. <br/><b>For instance <see cref="User"/> - Users</b></param>
    public UserOwnershipFilter(IEntityOwnershipService _ownershipService, string entityIdKey, string entityName)
    {
        this._ownershipService = _ownershipService;
        _entityIdKey = entityIdKey;
        _entityName = entityName;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        if (context.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Role).Any(c => c.Value == RoleNames.Admin || c.Value == RoleNames.Manager))
        {
            return;
        }
        var routeData = context.RouteData.Values;

        if (!routeData.TryGetValue(_entityIdKey, out var idObj) || !int.TryParse(idObj?.ToString(), out int entityId))
        {
            context.Result = new ForbidResult();
            return;
        }

        var userIdClaim = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
        {
            context.Result = new ForbidResult();
            return;
        }

        bool isOwner = await _ownershipService.IsUserOwnerAsync(userId, entityId, _entityName);
        if (!isOwner)
        {
            context.Result = new ForbidResult();
        }
    }


}
