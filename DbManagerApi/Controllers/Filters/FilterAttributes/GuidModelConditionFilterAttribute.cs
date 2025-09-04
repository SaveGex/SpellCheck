using Microsoft.AspNetCore.Mvc;

namespace DbManagerApi.Controllers.Filters.FilterAttributes;

public class GuidModelConditionFilterAttribute : TypeFilterAttribute
{
    public GuidModelConditionFilterAttribute() : base(typeof(GuidModuleConditionFilter))
    {
    }
}
