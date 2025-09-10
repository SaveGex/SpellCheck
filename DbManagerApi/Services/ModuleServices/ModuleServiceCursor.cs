using DbManagerApi.Services.Converters;
using DbManagerApi.Services.Interfaces;
using FluentResults;
using Infrastructure;
using Infrastructure.Models.ModelsDTO;
using Microsoft.EntityFrameworkCore;
using Mono.TextTemplating;
using MR.AspNetCore.Pagination;
using MR.EntityFrameworkCore.KeysetPagination;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using Module = Infrastructure.Models.Module;
namespace DbManagerApi.Services.ModuleServices;

public class ModuleServiceCursor : ModuleService, IGetModulesKeySetPaginationAsync
{
    private IPaginationService _paginationService;

    public ModuleServiceCursor(SpellTestDbContext context, IPaginationService paginationService) 
        : base(context)
    {
        _paginationService = paginationService;
    }

    public virtual Task<string> GetCursorBase64StringAsync(ModuleResponseDTO? cursorElement, string? propName)
    {
        PropertyInfo? propInfo = _context.Modules.GetType().GetProperty(propName ?? string.Empty);
        object? propValue;
        
        propValue = propInfo?.GetValue(cursorElement);

        if(propValue is null)
        {
            propValue = nameof(Module.Id);
        }

        return Task.FromResult(
            cursorElement?.Id.ToString() ?? "No more content..."
            );
    }

    public virtual async Task<Result<KeysetPaginationResult<ModuleResponseDTO>>> GetModulesKeySetPaginationAsync(string? after, string? propName, int? limit, int? moduleId, bool? reverse, int? wordsIncludeNumber)
    {
        {
            int afterInt;
            if (int.TryParse(after, out afterInt) && afterInt > await _context.Modules.MaxAsync(m => m.Id))
            {
                int MaxId = await _context.Modules.MaxAsync(m => m.Id);
                after = MaxId.ToString();
            }
        }
        IQueryable<Module> query = _context.Modules;
        if (moduleId is not null)
        {
            query = query.Where(m => m.Id == moduleId);
        }

        if(wordsIncludeNumber is not null)
        {
            query = query.Include(m => m.Words);
        }
  
        Func<IQueryable<Module>, IQueryable<ModuleResponseDTO>> map = (tIn) => tIn.Select(m => MapToDTO(m, wordsIncludeNumber));



        KeysetQueryModel queryModel = new KeysetQueryModel()
        {

            Size = limit ?? 20,
            After = after,
        };

        Action<KeysetPaginationBuilder<Module>> actionKeysetPaginationBuilder;
        try
        {
            actionKeysetPaginationBuilder = CreateActionKeysetPaginationBuilder(propName, reverse);
        }
        catch
        {
            actionKeysetPaginationBuilder = CreateActionKeysetPaginationBuilder(nameof(Module.Id), reverse);
        }

        KeysetPaginationResult< ModuleResponseDTO> result = await _paginationService.KeysetPaginateAsync(
            query,
            actionKeysetPaginationBuilder,
            async id => await _context.Modules.FindAsync(int.Parse(id)),
            map,
            queryModel: queryModel
        );
        
        return Result.Ok(result);
    }

    private Action<KeysetPaginationBuilder<Module>> CreateActionKeysetPaginationBuilder(string? propName, bool? reverse)
    {
        var parameter = Expression.Parameter(typeof(Module), "m");
        var property = Expression.PropertyOrField(parameter, propName ?? nameof(Module.Id));
        var propertyType = property.Type;

        var lambdaType = typeof(Func<,>).MakeGenericType(typeof(Module), propertyType);
        var lambda = Expression.Lambda(lambdaType, property, parameter);

        return builder =>
        {
            var ascendingMethod = typeof(KeysetPaginationBuilder<Module>)
                .GetMethods()
                .First(m => m.Name == "Ascending" && m.IsGenericMethod)
                .MakeGenericMethod(propertyType);

            var descendingMethod = typeof(KeysetPaginationBuilder<Module>)
                .GetMethods()
                .First(m => m.Name == "Descending" && m.IsGenericMethod)
                .MakeGenericMethod(propertyType);

            var propertyId = Expression.Lambda<Func<Module, int>>(
                Expression.PropertyOrField(parameter, nameof(Module.Id)),
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
