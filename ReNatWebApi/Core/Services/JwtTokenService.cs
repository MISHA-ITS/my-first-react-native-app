using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Domain.Entities.Identity;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Core.Services;

public class JwtTokenService
    (
    IConfiguration configuration, //дає доступ до налаштувань з appsettings.json
    UserManager<UserEntity> userManager //служба для роботи з користувачами Identity
    ) : IJwtTokenService
{
    public async Task<string> CreateTokenAsync(UserEntity user)
    {
        //отримуємо секретний ключ із конфігурації
        var key = configuration["Jwt:Key"];

        //створюємо список даних про користувача (claims), які будуть збережені у токені
        var claims = new List<Claim>
        {
            new Claim("email", user.Email),
            new Claim("name", $"{user.LastName} {user.FirstName}"),
            new Claim("image", $"{user.Image}")
        };

        //отримуємо ролі користувача і додаємо їх у список claims
        foreach (var role in await userManager.GetRolesAsync(user))
        {
            claims.Add(new Claim("roles", role));
        }

        //ключ для підпису токена - перетворюємо у байти
        var keyBytes = System.Text.Encoding.UTF8.GetBytes(key);

        //створюємо об'єкт для підпису токена
        var symmetricSecurityKey = new SymmetricSecurityKey(keyBytes);

        //вказуємо ключ і алгоритм підпису токена
        var signingCredentials = new SigningCredentials(
            symmetricSecurityKey,
            SecurityAlgorithms.HmacSha256); //алгоритм шифрування

        //створюємо токен
        var jwtSecurityToken = new JwtSecurityToken(
            claims: claims, //список параметрів у токені, які є доступні
            expires: DateTime.UtcNow.AddDays(7), // термін дії токена - після цього часу токен буде недійсний
            signingCredentials: signingCredentials); //параметри підпису токена

        //генеруємо рядок токена у форматі JWT
        string token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

        //повертаємо згенерований токен
        return token;
    }
}