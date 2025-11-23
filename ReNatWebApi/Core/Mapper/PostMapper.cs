using AutoMapper;
using Core.Models.Post;
using Domain.Entities;

namespace Core.Mapper;

public class PostMapper : Profile
{
    public PostMapper()
    {
        CreateMap<CreatePostModel, PostEntity>()
            .ForMember(dest => dest.Image, opt => opt.Ignore());

        CreateMap<PostEntity, PostModel>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));
    }
}
