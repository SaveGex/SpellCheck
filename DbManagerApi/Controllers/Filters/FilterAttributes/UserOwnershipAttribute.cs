using DomainData.Models;
using Microsoft.AspNetCore.Mvc;

namespace DbManagerApi.Controllers.Filters.FilterAttributes;

public class UserOwnershipAttribute : TypeFilterAttribute
{
    /// <summary>
    /// Specifies that a user ownership of an entity or it's have special permission (admin, manager)
    /// </summary>
    /// <param name="entityIdKey">The key representing a <see cref="int"/> variable from route which is contains Id of an entity</param>
    /// <param name="entityName">The name in plural of the entity to which the filter applies. <br/><b>For instance <see cref="User"/> - Users</b></param>
    public UserOwnershipAttribute(string entityIdKey, string entityName)
        : base(typeof(UserOwnershipFilter))
    {
        Arguments = new object[] { entityIdKey, entityName };
    }
}
