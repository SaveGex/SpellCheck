using Infrastructure.Models;
using Infrastructure.Models.ModelsDTO;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Data.Common;
using System.Security.Cryptography;
using System.Text;

namespace DbManagerApi.Controllers.Filters;

public class GuidModuleConditionFilter : IActionFilter
{
    public void OnActionExecuted(ActionExecutedContext context)
    {
        //throw new NotImplementedException();
    }
    //тут виходить хуйня що при передаванні назви ідентифікатора то він генерується як повторний навіть якщо раніше тикого не задавав
    public void OnActionExecuting(ActionExecutingContext context)
    {
        foreach(object? contextObject in context.ActionArguments.Values)
        {
            if(contextObject is ModuleCreateDTO or ModuleUpdateDTO)
            {
                dynamic module = (dynamic)contextObject;
                switch (module.IdentifierName)
                {
                    case null: return;
                    case not null: module.Identifier = CreateGuidByName(module.IdentifierName); return;
                }
            }
        }
    }

    private Guid CreateGuidByName(string name)
    {
        Guid Identifier;
        using (var md5 = MD5.Create())
        {
            byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(name));
            Identifier = new Guid(hash);
        }
        return Identifier;
    }
}
