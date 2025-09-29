using DomainData.Interfaces;
using DomainData.Models;
using Infrastructure.DB;
using System.Data;

namespace Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    public SpellTestDbContext Context { get; init; }

    public RoleRepository(SpellTestDbContext spellTestDbContext)
    {
        Context = spellTestDbContext;
    }


    public async Task<Role> CreateRoleAsync(Role dto)
    {
        if (Context.Roles
            .Select(r => r.Name.ToLower())
            .Contains(dto.Name.ToLower()))
        {
            throw new Exception("Role with the same name already exists");
        }

        Role result = Context.Roles.Add(dto).Entity;

        await Context.SaveChangesAsync();
        return result;
    }


    public async Task<Role> DeleteRoleAsync(int roleId)
    {
        Role? role = await Context.Roles.FindAsync(roleId);
        if (role is null)
        {
            throw new Exception("Role by this id does not found");
        }

        Context.Roles.Remove(role);
        await Context.SaveChangesAsync();
        return role;
    }


    public Task<IEnumerable<Role>> GetRolesAsync()
    {
        IEnumerable<Role> result = Context.Roles;
        return Task.FromResult(
            result);
    }


    public async Task<Role> GetRoleAsync(int roleId)
    {
        Role? role = await Context.Roles.FindAsync(roleId);
        if (role is null)
        {
            throw new Exception("Role by this id does not found");
        }
        return role;
    }

    public async Task<Role> UpdateRoleAsync(Role dto)
    {
        Role result = Context.Roles.Update(dto).Entity;
        await Context.SaveChangesAsync();
        return result;
    }


}
