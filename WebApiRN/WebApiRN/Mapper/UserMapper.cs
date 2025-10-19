using AutoMapper;
using WebApiRN.Data.Entities.Identity;
using WebApiRN.Models;

namespace WebApiRN.Mapper;

public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<SeederUserModel, UserEntity>()
            .ForMember(opt => opt.UserName, opt => opt.MapFrom(x => x.Email));
        CreateMap<RegisterModel, UserEntity>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
        CreateMap<UserEntity, UserModel>();
        ;
    }
}
