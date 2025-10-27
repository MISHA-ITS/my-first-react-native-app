using AutoMapper;
using Core.Constants;
using Core.Interfaces;
using Core.Models.Account;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ReNatWebApi.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class AccountController(IJwtTokenService jwtTokenService,
        IMapper mapper, IImageService imageService,
        UserManager<UserEntity> userManager) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        //знаходимо користувача за email
        var user = await userManager.FindByEmailAsync(model.Email);

        //перевіряємо, чи існує користувач і чи правильний пароль
        if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
        {
            //генеруємо JWT токен для користувача
            var token = await jwtTokenService.CreateTokenAsync(user);
            //повертаємо токен у відповіді
            return Ok(new { Token = token });
        }
        //якщо користувача не знайдено або пароль неправильний - повертаємо помилку 401 Unauthorized
        return Unauthorized("Invalid email or password");
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromForm] RegisterModel model)
    {
        //мапінг моделі реєстрації у сутність користувача
        //створюється новий об’єкт користувача, готовий до збереження в базі
        var user = mapper.Map<UserEntity>(model);

        //збереження зображення користувача за допомогою сервісу збереження зображень
        user.Image = await imageService.SaveImageAsync(model.ImageFile!);

        //створення користувача в базі даних з вказаним паролем
        var result = await userManager.CreateAsync(user, model.Password);

        //якщо створення користувача пройшло успішно
        if (result.Succeeded)
        {
            //додаємо користувача до ролі "User"
            await userManager.AddToRoleAsync(user, Roles.User);
            //генеруємо JWT токен для нового користувача
            var token = await jwtTokenService.CreateTokenAsync(user);
            //повертаємо токен у відповіді
            return Ok(new
            {
                Token = token
            });
        }
        //якщо створення користувача не вдалося
        else
        {
            //повертаємо помилку 400 Bad Request з деталями
            return BadRequest(new
            {
                status = 400,
                isValid = false,
                errors = "Registration failed"
            });
        }
    }
}
