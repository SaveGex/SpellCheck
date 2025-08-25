using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace DbManagerApi.Controllers.Filters.FilterAttributes;

public class GuidModelConditionFilterAttribute : TypeFilterAttribute
{
    public GuidModelConditionFilterAttribute() : base(typeof(GuidModuleConditionFilter))
    {
    }
}
