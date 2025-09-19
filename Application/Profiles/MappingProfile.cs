
using Application.Interfaces;
using Application.ModelsDTO;
using AutoMapper;
using DomainData.Models;
using DomainData.Records;

namespace Application.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserResponseDTO>();
        CreateMap<UserCreateDTO, User>();
        CreateMap<UserUpdateDTO, User>();

        CreateMap<KeysetPaginationAfterResult<User>, KeysetPaginationAfterResult<UserResponseDTO>>();

        CreateMap<Module, ModuleResponseDTO>();
        CreateMap<ModuleCreateDTO, Module>();
        CreateMap<ModuleUpdateDTO, Module>();

        CreateMap<KeysetPaginationAfterResult<Module>, KeysetPaginationAfterResult<ModuleResponseDTO>>();

        CreateMap<Word, WordResponseDTO>();
        CreateMap<WordCreateDTO, Word>();
        CreateMap<WordUpdateDTO, Word>();

        CreateMap<Role, RoleResponseDTO>();
        CreateMap<RoleCreateDTO, Role>();
        CreateMap<RoleUpdateDTO, Role>();

        CreateMap<DifficultyLevel, DifficultyLevelResponseDTO>();
        CreateMap<DifficultyLevelCreateDTO, DifficultyLevel>();
        CreateMap<DifficultyLevelUpdateDTO, DifficultyLevel>();

        CreateMap<Friend, FriendResponseDTO>();
        CreateMap<FriendCreateDTO, Friend>();
        CreateMap<FriendUpdateDTO, Friend>();

        CreateMap<KeysetPaginationAfterResult<Friend>, KeysetPaginationAfterResult<FriendResponseDTO>>();

    }
}
