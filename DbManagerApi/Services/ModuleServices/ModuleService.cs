using DbManagerApi.Services.Abstracts;
using DbManagerApi.Services.Extentions;
using DbManagerApi.Services.WordServices;
using FluentResults;
using Infrastructure;
using Infrastructure.Models;
using Infrastructure.Models.ModelsDTO;
using Microsoft.EntityFrameworkCore;
using MR.AspNetCore.Pagination;
using System.Linq.Expressions;
using Module = Infrastructure.Models.Module;

namespace DbManagerApi.Services.ModuleServices;

public class ModuleService : ModuleServiceAbstract
{

    protected readonly SpellTestDbContext _context;

    public ModuleService(SpellTestDbContext context)
    {
        _context = context;
    }

    public static ModuleResponseDTO MapToDTO(Module module, int? wordsNumber = 0)
    {
        return new ModuleResponseDTO
        {
            Id = module.Id,
            IdentifierName = module.IdentifierName,
            Identifier = module.Identifier,
            Name = module.Name,
            AuthorId = module.AuthorId,
            CreatedAt = module.CreatedAt,
            Words = module.Words.OrderBy(w => w.CreatedAt)
                            .Take(wordsNumber ?? 0)
                            .Select(w => WordService.MapToDTO(w))
                            .AsCollection()
        };
    }

    public override async Task<Result<ModuleResponseDTO>> CreateEntityAsync(ModuleCreateDTO dto)
    {
        Module module = new();
        _context.Entry(module).CurrentValues.SetValues(dto);

        ParameterExpression moduleParam = Expression.Parameter(typeof(Module), "module");

        MemberExpression moduleIdentifier = Expression.Property(moduleParam, nameof(Module.Identifier));

        ConstantExpression dtoIdentifier = Expression.Constant(module.Identifier, typeof(Guid));

        BinaryExpression equalExpression = Expression.Equal(moduleIdentifier, dtoIdentifier);

        Expression<Func<Module, bool>> moduleEqualsPredicateExpression =
            Expression.Lambda<Func<Module, bool>>(equalExpression, moduleParam);

        if (await _context.Modules.AnyAsync(moduleEqualsPredicateExpression))
        {
            return Result.Fail("Module with the same identifier already exists.");
        }

        await SetWordsCollectionAsync(ref module, dto);

        _context.Entry(module).State = EntityState.Added;
        await _context.SaveChangesAsync();

        return Result.Ok(MapToDTO(module));
    }

    public override async Task<Result<ModuleResponseDTO>> DeleteEntityAsync(int moduleId)
    {
        Module? module = await _context.Modules.FindAsync(moduleId);

        if (module is null)
        {
            return Result.Fail("Module not found.");
        }

        _context.Entry(module).State = EntityState.Deleted;
        await _context.SaveChangesAsync();

        return Result.Ok(MapToDTO(module));
    }

    /// <summary>
    /// <b>Does not contain words inside the modules</b> - perfomance optimization
    /// </summary>
    /// <returns><see cref="IEnumerable{ModuleResponseDTO}"/> represents as <seealso cref="Result{IEnumerable{ModuleResponseDTO}}"/> including state's executing does it completed succesfully or not</returns>
    public async Task<Result<IEnumerable<ModuleResponseDTO>>> GetAllModulesAsync()
    {
        IEnumerable<Module> modules = await _context.Modules
            .ToListAsync();

        if (!modules.Any())
        {
            return Result.Fail<IEnumerable<ModuleResponseDTO>>("No modules found. Or Sequence does not contain any modules");
        }

        return Result.Ok(modules.Select(m => MapToDTO(m)).AsEnumerable());
    }

    public override async Task<Result<IEnumerable<ModuleResponseDTO>>> GetModulesSequenceAsync(string? propName, int? limit, int? moduleId, bool? reverse, int? wordsIncludeNumber)
    {
        string orderBy = string.IsNullOrWhiteSpace(propName) ? nameof(Module.Id) : propName;
        int take = Math.Clamp(limit ?? 100, 1, 1000);
        int startId = moduleId ?? 1;
        bool descending = reverse ?? false;
        int wordsNumber = wordsIncludeNumber ?? 0;

        IQueryable<Module> query = _context.Modules
            .Include(m => m.Words)
            .AsQueryable()
            .Where(m => m.Id >= startId);

        query = query.OrderByProperty(orderBy, descending);

        IEnumerable<ModuleResponseDTO> modules = (
                await query
                .Include(m => m.Words)
                .Take(take)
                .ToListAsync()
            ).Select(m => MapToDTO(m, wordsNumber));


        return Result.Ok(modules);
    }

    public override async Task<Result<ModuleResponseDTO>> GetEntityByIdAsync(int moduleId)
    {
        Module? module = await _context.Modules
            .Include(m => m.Words)
            .FirstOrDefaultAsync(m => m.Id == moduleId);
        if (module is null)
        {
            return Result.Fail("Module not found.");
        }

        return Result.Ok(MapToDTO(module));
    }

    public override async Task<Result<ModuleResponseDTO>> UpdateEntityAsync(ModuleUpdateDTO dto, int moduleId)
    {
        Module? module = await _context.Modules.FindAsync(moduleId);
        if (module is null)
        {
            return Result.Fail("Module does not found.");
        }
        if (dto.Identifier is not null &&
            await _context.Modules.AnyAsync(m => dto.Identifier == m.Identifier))
        {
            return Result.Fail("Module with the same unique identifier already exists");
        }
        //if identifier was not generated inner the filter
        else if (dto.Identifier is null)
        {
            //change the null value for avoid NullReferenceException during SetValues()
            dto.Identifier = module.Identifier;
        }


        _context.Entry(module).CurrentValues.SetValues(dto);
        _context.Entry(module).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return Result.Ok(MapToDTO(module));
    }

    private Task SetWordsCollectionAsync(ref Module target, ModuleCreateDTO source)
    {
        if (source.Words is null || !source.Words.Any())
        {
            return Task.CompletedTask;
        }

        foreach (WordCreateDTO moduleCreateDTO in source.Words)
        {
            Word word = new Word();
            _context.Entry(word).CurrentValues.SetValues(moduleCreateDTO);
            target.Words.Add(word);
        }

        return Task.CompletedTask;
    }

}