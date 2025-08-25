using DbManagerApi.Controllers.Filters.FilterAttributes;
using DbManagerApi.Services.Interfaces;
using FluentResults;
using Infrastructure;
using Infrastructure.Models;
using Infrastructure.Models.ModelsDTO;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;
using System.Linq.Expressions;
using PropertyInfo = System.Reflection.PropertyInfo;

namespace DbManagerApi.Services;

public class ModuleService : IModuleService
{

    private readonly SpellTestDbContext _context;

    public ModuleService(SpellTestDbContext context)
    {
        _context = context;
    }

    public static ModuleResponseDTO MapToDTO(Module module)
    {
        return new ModuleResponseDTO
        {
            Id = module.Id,
            IdentifierName = module.IdentifierName,
            Identifier = module.Identifier,
            Name = module.Name,
            AuthorId = module.AuthorId,
            CreatedAt = module.CreatedAt,
            Words = module.Words.Select(w => WordService.MapToDTO(w)).ToList()
        };
    }

    public async Task<Result<ModuleResponseDTO>> CreateModuleAsync(ModuleCreateDTO dto)
    {
        Module module = new Module();
        _context.Entry(module).CurrentValues.SetValues(dto);

        // Параметр для Lambda (module => ...)
        ParameterExpression moduleParam = Expression.Parameter(typeof(Module), "module");

        // Доступ до властивості module.Identifier
        MemberExpression moduleIdentifier = Expression.Property(moduleParam, nameof(Module.Identifier));

        // Константа dto.Identifier
        ConstantExpression dtoIdentifier = Expression.Constant(module.Identifier, typeof(Guid));

        // Вираз module.Identifier == dto.Identifier
        BinaryExpression equalExpression = Expression.Equal(moduleIdentifier, dtoIdentifier);

        // Lambda: module => module.Identifier == dto.Identifier
        Expression<Func<Module, bool>> moduleEqualsPredicateExpression =
            Expression.Lambda<Func<Module, bool>>(equalExpression, moduleParam);

        // Використовуємо в AnyAsync
        if (await _context.Modules.AnyAsync(moduleEqualsPredicateExpression))
        {
            return Result.Fail("Module with the same identifier already exists.");
        }

        await SetWordsCollectionAsync(ref module, dto);

        _context.Entry(module).State = EntityState.Added;
        await _context.SaveChangesAsync();

        return Result.Ok(MapToDTO(module));
    }

    public async Task<Result<ModuleResponseDTO>> DeleteModuleAsync(int moduleId)
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

        if(!modules.Any())
        {
            return Result.Fail<IEnumerable<ModuleResponseDTO>>("No modules found. Or Sequence does not contain any modules");
        }

        return Result.Ok(modules.Select(m => MapToDTO(m)).AsEnumerable());
    }

    public async Task<Result<ModuleResponseDTO>> GetModuleByIdAsync(int moduleId)
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

    public async Task<Result<ModuleResponseDTO>> UpdateModuleAsync(ModuleUpdateDTO dto, int moduleId)
    {
        Module? module = await _context.Modules.FindAsync(moduleId);
        if (module is null)
        {
            return Result.Fail("Module does not found.");
        }
        if(dto.Identifier is not null && 
            await _context.Modules.AnyAsync(m => dto.Identifier == m.Identifier))
        {     
            return Result.Fail("Module with the same unique identifier already exists");
        }
        //if identifier was not generated inner the filter
        else if(dto.Identifier is null)
        {
            //change the null value for avoid NullReferenceException during SetValues()
            dto.Identifier = module.Identifier;
        }


        _context.Entry(module).CurrentValues.SetValues(dto);
        _context.Entry(module).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return Result.Ok(MapToDTO(module));
    }

    public Task SetWordsCollectionAsync(ref Module target, ModuleCreateDTO source)
    {
        if(source.Words is null || !source.Words.Any())
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
