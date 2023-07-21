using API.Dtos.User;
using API.Entities;
using AutoMapper;

namespace API.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<AppUser, UserDetailsDto>();
        CreateMap<UpdateUserDto, AppUser>();
    }
}