using AutoMapper;
using Core.Models.Account;
using Core.Models.Seeder;
using Core.Models.User;
using Domain.Entities.Identity;

namespace Core.Mapper;

public class UserMapper : Profile
{
    public UserMapper()
    {
        // Seeder mapping (для початкових користувачів)
        CreateMap<SeederUserModel, UserEntity>()
            .ForMember(opt => opt.UserName, opt => opt.MapFrom(x => x.Email));

        // Register mapping (для реєстрації користувача)
        CreateMap<RegisterModel, UserEntity>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Image, opt => opt.Ignore()); // файл зображення обробляється окремо

        // User mapping (для відображення інформації про користувача)
        CreateMap<UserEntity, UserModel>()
            .ForMember(dest => dest.Roles, opt => opt.Ignore());
        CreateMap<UserModel, UserEntity>();
    }
}
