using Application.ModelsDTO;
using AutoMapper;
using DomainData.Models;
using Microsoft.AspNetCore.Identity;

namespace Application.Profiles.Resolvers;

public class PasswordHashResolver : 
    IValueResolver<UserLoginDTO, User, string>,
    IValueResolver<UserRegisterDTO, User, string>,
    IValueResolver<UserUpdateDTO, User, string>
{
    private IPasswordHasher<User> PasswordHasher { get; init; }

    public PasswordHashResolver(IPasswordHasher<User> passwordHasher)
    {
        PasswordHasher = passwordHasher;
    }
    public string Resolve(UserLoginDTO source, User destination, string destMember, ResolutionContext context)
    {
        return PasswordHasher.HashPassword(destination, source.Password);
    }

    public string Resolve(UserRegisterDTO source, User destination, string destMember, ResolutionContext context)
    {
        return PasswordHasher.HashPassword(destination, source.Password);
    }

    public string Resolve(UserUpdateDTO source, User destination, string destMember, ResolutionContext context)
    {
        return PasswordHasher.HashPassword(destination, source.Password);
    }

}
