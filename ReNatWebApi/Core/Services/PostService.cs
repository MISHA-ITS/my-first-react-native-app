using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Interfaces;
using Core.Models.Post;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Services;

public class PostService(AppDbContext appDbContext,
IMapper mapper, IImageService imageService) : IPostService
{
    public async Task<PostModel> CreateAsync(CreatePostModel dto, long userId)
    {
        var post = mapper.Map<PostEntity>(dto);
        post.UserId = userId;
        
        if (dto.Image != null)
        {
            post.Image = await imageService.SaveImageAsync(dto.Image);
        }

        await appDbContext.Posts.AddAsync(post);
        await appDbContext.SaveChangesAsync();

        return mapper.Map<PostModel>(post);
    }

    public async Task<List<PostModel>> GetUserPostsAsync(long userId)
    {
        return await appDbContext.Posts
            .Where(p => p.UserId == userId)
            .ProjectTo<PostModel>(mapper.ConfigurationProvider)
            .ToListAsync();
    }
}
