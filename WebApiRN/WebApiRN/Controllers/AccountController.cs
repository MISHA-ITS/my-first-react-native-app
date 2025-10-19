using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiRN.Constants;
using WebApiRN.Data.Entities.Identity;
using WebApiRN.Interfaces;
using WebApiRN.Models;

namespace WebApiRN.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class AccountController(IJwtTokenService jwtTokenService,
        IMapper mapper, IImageService imageService,
        UserManager<UserEntity> userManager) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await userManager.FindByEmailAsync(model.Email);
        if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
        {
            var token = await jwtTokenService.CreateTokenAsync(user);
            return Ok(new { Token = token });
        }
        return Unauthorized("Invalid email or password");
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromForm] RegisterModel model)
    {
        var user = mapper.Map<UserEntity>(model);

        user.Image = await imageService.SaveImageAsync(model.ImageFile!);

        var result = await userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, Roles.User);
            var token = await jwtTokenService.CreateTokenAsync(user);
            return Ok(new
            {
                Token = token
            });
        }
        else
        {
            return BadRequest(new
            {
                status = 400,
                isValid = false,
                errors = "Registration failed"
            });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await userManager.Users.ToListAsync();
        var userDtos = mapper.Map<List<UserModel>>(users);
        return Ok(userDtos);
    }
}
