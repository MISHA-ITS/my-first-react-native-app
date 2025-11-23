using Core.Models.Post;
using Domain.Entities;

namespace Core.Interfaces;

public interface IPostService
{
    Task<PostModel> CreateAsync(CreatePostModel dto, long userId);
    Task<List<PostModel>> GetUserPostsAsync(long userId);
}
