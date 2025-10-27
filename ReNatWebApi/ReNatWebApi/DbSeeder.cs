using AutoMapper;
using Core.Constants;
using Core.Interfaces;
using Core.Models.Seeder;
using Core.Services;
using Domain;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace MyAPI;

public static class DbSeeder
{
    public static async Task SeedData(this WebApplication webApplication)
    {
        //Створюємо область (scope) для отримання сервісів з DI контейнера
        using var scope = webApplication.Services.CreateScope();
        //Цей об'єкт буде повертати посилання на конткест, який зараєстрвоано в Program.cs
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        //Отримуємо менеджери ролей і користувачів, а також мапер
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<RoleEntity>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserEntity>>();
        var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

        //Застосовуємо всі міграції до бази даних
        context.Database.Migrate();

        //Перевіряємо, чи є ролі в базі даних
        if (!context.Roles.Any())
        {
            //Якщо ролей немає, створюємо їх на основі констант
            foreach (var roleName in Roles.AllRoles)
            {
                //створюємо нову роль
                var result = await roleManager.CreateAsync(new(roleName));
                //Якщо створення ролі не вдалося
                if (!result.Succeeded)
                {
                    //Виводимо повідомлення про помилку у консоль
                    Console.WriteLine("Error Create Role {0}", roleName);
                }
            }
        }

        //Перевіряємо, чи є користувачі в базі даних
        if (!context.Users.Any())
        {
            //Отримуємо сервіс для роботи з зображеннями
            var imageService = scope.ServiceProvider.GetRequiredService<IImageService>();
            //Якщо користувачів немає, завантажуємо їх з JSON файлу
            var jsonFile = Path.Combine(Directory.GetCurrentDirectory(), "Helpers", "JsonData", "Users.json");
            //Перевіряємо, чи існує файл
            if (File.Exists(jsonFile))
            {
                //Читаємо вміст файлу асинхронно
                var jsonData = await File.ReadAllTextAsync(jsonFile);
                
                try
                {
                    //Парсимо JSON дані у список моделей користувачів
                    var users = JsonSerializer.Deserialize<List<SeederUserModel>>(jsonData);
                    //Якщо парсинг пройшов успішно і список не порожній
                    foreach (var user in users)
                    {
                        //Мапимо модель користувача у сутність користувача
                        var entity = mapper.Map<UserEntity>(user);
                        //Встановлюємо додаткові властивості
                        entity.UserName = user.Email;
                        entity.Image = await imageService.SaveImageFromUrlAsync(user.Image);
                        //Створюємо користувача з вказаним паролем
                        var result = await userManager.CreateAsync(entity, user.Password);
                        //Якщо створення користувача не вдалося
                        if (!result.Succeeded)
                        {
                            //Виводимо повідомлення про помилку у консоль
                            Console.WriteLine("Error Create User {0}", user.Email);
                            continue;
                        }
                        //Додаємо користувача до вказаних ролей
                        foreach (var role in user.Roles)
                        {
                            //Перевіряємо, чи існує роль
                            if (await roleManager.RoleExistsAsync(role))
                            {
                                //Додаємо користувача до ролі
                                await userManager.AddToRoleAsync(entity, role);
                            }
                            //Якщо роль не знайдена
                            else
                            {
                                //Виводимо повідомлення про помилку у консоль
                                Console.WriteLine("Not Found Role {0}", role);
                            }
                        }
                    }

                }
                //Ловимо помилки парсингу JSON
                catch (Exception ex)
                {
                    //Виводимо повідомлення про помилку у консоль
                    Console.WriteLine("Error Json Parse Data {0}", ex.Message);
                }
            }
            //Якщо файл не знайдено
            else
            {
                //Виводимо повідомлення про помилку у консоль
                Console.WriteLine("Not Found File Users.json");
            }
        }
    }
}