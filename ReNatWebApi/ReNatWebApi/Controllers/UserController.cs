using AutoMapper;
using Core.Interfaces;
using Core.Models.User;
using Domain;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ReNatWebApi.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]

public class UserController(IMapper mapper, IImageService imageService,
        UserManager <UserEntity> userManager) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<ActionResult<UserModel>> GetUserById(long id)
    {
        var user = await userManager.FindByIdAsync(id.ToString());
        if (user == null)
            return NotFound();

        var model = mapper.Map<UserModel>(user);

        // Отримуємо ролі користувача
        var roles = await userManager.GetRolesAsync(user);
        model.Roles = roles.ToArray();

        return Ok(model);
    }

}
