using AutoMapper;
using Core.Constants;
using Core.Interfaces;
using Core.Models.Account;
using Core.Models.User;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ReNatWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController(IJwtTokenService jwtTokenService,
        IMapper mapper, IImageService imageService,
        UserManager<UserEntity> userManager) : ControllerBase
{
    [HttpPost("login")]
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

    [HttpPost("register")]
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

    [Authorize]
    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        // Отримуємо ID користувача з JWT токена
        var userId = User.FindFirst("id")?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        // Знаходимо користувача в базі
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
            return NotFound();

        // Мапимо сутність у DTO
        var model = mapper.Map<UserModel>(user);

        // Отримуємо ролі користувача
        var roles = await userManager.GetRolesAsync(user);
        model.Roles = roles.ToArray();

        // Додаємо повний шлях до зображення (якщо є)
        if (!string.IsNullOrEmpty(model.Image))
        {
            // формує повний URL на основі імені файлу
            var request = HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";
            model.Image = $"{baseUrl}/images/{model.Image}";
        }

        // Повертаємо чистий DTO
        return Ok(model);
    }
}
