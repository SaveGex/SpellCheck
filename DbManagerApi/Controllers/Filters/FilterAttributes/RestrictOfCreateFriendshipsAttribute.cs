using Application.ModelsDTO;
using FluentResults;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace DbManagerApi.Controllers.Filters.FilterAttributes
{
    /// <summary>
    /// The goal is: exclude opportunity's users to create relationships between other users.
    /// Checks such models as <see cref="FriendUpdateDTO"/> <see cref="FriendCreateDTO"/> and their properties of ids' users
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class RestrictOfCreateFriendshipsAttribute 
        : ActionFilterAttribute
    {
        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            object? friend = null;
            foreach (var item in context.ActionArguments.Values)
            {
                if(item is FriendCreateDTO or FriendUpdateDTO)
                {
                    friend = item;
                    break;
                }
            }

            if (friend is null && context.HttpContext.User.Identity?.IsAuthenticated != true)
            {
                return base.OnActionExecutionAsync(context, next);
            }

            Claim? idClaim = context.HttpContext.User.Claims
                .Where(c => c.Type == ClaimTypes.NameIdentifier)
                .FirstOrDefault();

            if (idClaim == null) 
            {
                context.Result = new BadRequestObjectResult("User is authenticated but don't have any Id");
                return base.OnActionExecutionAsync(context, next);
            }

            int userId = int.Parse(idClaim.Value);

            if (friend is FriendCreateDTO friendCreate) {
                if (friendCreate.ToIndividualId != userId && friendCreate.FromIndividualId != userId)
                {
                    context.Result = new BadRequestObjectResult("User cannot create relationship between other users");
                }
            }
            if (friend is FriendUpdateDTO friendUpdate)
            {
                if (friendUpdate.ToIndividualId != userId && friendUpdate.FromIndividualId != userId)
                {
                    context.Result = new BadRequestObjectResult("User cannot create relationship between other users");
                }
            }


            return base.OnActionExecutionAsync(context, next);
        }
    }
}
