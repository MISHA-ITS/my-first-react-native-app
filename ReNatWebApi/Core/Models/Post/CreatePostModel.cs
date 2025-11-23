using Microsoft.AspNetCore.Http;

namespace Core.Models.Post;

public class CreatePostModel
{
    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public IFormFile? Image { get; set; }
}