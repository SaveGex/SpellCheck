using DbManagerApi.Services.WordServices;
using DomainData;
using Microsoft.EntityFrameworkCore;
using MR.AspNetCore.Pagination;
using MR.EntityFrameworkCore.KeysetPagination;
using System.Linq.Expressions;
using DomainData.Interfaces;
using DomainData.Models;
using DomainData.Records;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DbManagerApi.Services.ModuleServices;

public sealed class ModuleRepository : IModuleRepository
{

    private readonly SpellTestDbContext _context;

    private readonly IPaginationService _paginationService;

    public ModuleRepository(SpellTestDbContext context, IPaginationService paginationService)
    {
        _context = context;
        _paginationService = paginationService;
    }


    public async Task<bool> AnyAsync(Module module) 
        => await _context.Modules.FindAsync(module) is not null;
    

    private Task<string> GetCursorBase64StringAsync(Module? cursorElement)
    {
        return Task.FromResult(
            cursorElement?.Id.ToString() ?? "No more content..."
            );
    }

    public async Task<KeysetPaginationAfterResult<Module>> GetModulesKeySetPaginationAsync(string? after, string? propName, int? limit, int? moduleId, bool? reverse, int? wordsIncludeNumber)
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

        if (wordsIncludeNumber is not null)
        {
            query = query.Include(m => m.Words.Take(wordsIncludeNumber ?? 0));
        }


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

        KeysetPaginationResult<Module> result = await _paginationService.KeysetPaginateAsync(
            query,
            actionKeysetPaginationBuilder,
            async id => await _context.Modules.FindAsync(int.Parse(id)),
            queryModel: queryModel
        );

        return new KeysetPaginationAfterResult<Module>(
            await GetCursorBase64StringAsync(
                result.Data.LastOrDefault()),
            result);
    }

    public async Task<Module> CreateModuleAsync(Module module)
    {

        _context.Modules.Add(module);
        await _context.SaveChangesAsync();

        return module;
    }

    public async Task<Module> DeleteModuleAsync(Module module)
    {
        _context.Modules.Remove(module);
        await _context.SaveChangesAsync();

        return module;
    }

    public async Task<Module> GetModuleByIdAsync(int moduleId)
    {
        Module? module = await _context.Modules
            .Include(m => m.Words)
            .FirstOrDefaultAsync(m => m.Id == moduleId);
        if (module is null)
        {
            throw new Exception("Module not found.");
        }

        return module;
    }

    public async Task<Module> UpdateModuleAsync(Module module)
    {
        _context.Modules.Update(module);
        await _context.SaveChangesAsync();

        return module;
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

    private Task SetWordsCollectionAsync(ref Module target, Module source)
    {
        if (source.Words is null || !source.Words.Any())
        {
            return Task.CompletedTask;
        }

        foreach (Word wordCreate in source.Words)
        {
            Word word = new Word();
            _context.Entry(word).CurrentValues.SetValues(wordCreate);
            target.Words.Add(word);
        }

        return Task.CompletedTask;
    }

}