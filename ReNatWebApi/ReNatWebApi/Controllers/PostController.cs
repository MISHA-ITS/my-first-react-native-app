using Core.Interfaces;
using Core.Models.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ReNatWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostController(IPostService PostService) : ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreatePostModel dto)
    {
        var userId = long.Parse(User.FindFirst("id")!.Value);

        var post = await PostService.CreateAsync(dto, userId);

        return Ok(post);
    }

    [Authorize]
    [HttpGet("AllMyPosts")]
    public async Task<IActionResult> GetMyPosts()
    {
        var userId = long.Parse(User.FindFirst("id")!.Value);
        var posts = await PostService.GetUserPostsAsync(userId);

        return Ok(posts);
    }
}
