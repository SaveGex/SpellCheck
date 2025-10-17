using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;
using System.Security.Claims;

namespace DbManagerApi.Controllers.Filters.FilterAttributes
{
    /// <summary>
    /// Bind user id to AuthorId property
    /// </summary>
    /// <typeparam name="TModel">Model to which </typeparam>
    public class BindingAuthorIdAttribute<TDestination> : ActionFilterAttribute
    {
        private string authorIdPropertyName { get; init; }

        private TDestination? destinationModel;

        public BindingAuthorIdAttribute(string authorIdPropertyName = "AuthorId")
        {
            this.authorIdPropertyName = authorIdPropertyName;
        }

        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            foreach (object? item in context.ActionArguments.Values)
            {
                if (item is TDestination)
                {
                    PropertyInfo? authorIdPropInfo = item.GetType().GetProperty(authorIdPropertyName);
                    if (authorIdPropInfo is null)
                    {
                        throw new Exception("Object does not contain property with specified name");
                    }

                    Claim userIdClaim = context.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).First();
                    if (userIdClaim is null)
                        throw new UnauthorizedAccessException("User does not have a valid NameIdentifier claim.");

                    int userId = int.Parse(userIdClaim.Value);

                    authorIdPropInfo.SetValue(item, userId);
                }
            }
            return base.OnActionExecutionAsync(context, next);
        }
    }
}
