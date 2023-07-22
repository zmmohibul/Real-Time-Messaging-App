using API.Dtos.Message;
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

        CreateMap<Message, MessageDto>()
            .ForMember(dest => dest.SenderUserName,
                opt => opt.MapFrom(src => src.Sender.UserName))
            .ForMember(dest => dest.RecipientUserName,
                opt => opt.MapFrom(src => src.Recipient.UserName));
    }
}