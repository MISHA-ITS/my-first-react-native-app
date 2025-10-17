using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApiRN.Data.Entities.Identity;

namespace WebApiRN.Data;

public static class Seeder
{
    public static async Task Seed(this WebApplication webApplication)
    {
        using var scope = webApplication.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppWebApiDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserEntity>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<RoleEntity>>();

        await dbContext.Database.MigrateAsync();

        // Seed Roles
        foreach (var role in Constants.Roles.AllRoles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new RoleEntity(role));
            }
        }

        // Seed Admin User
        if (!dbContext.Users.Any())
        {
            var adminUser = new UserEntity
            {
                UserName = "admin@mail.com",
                Email = "admin@mail.com",
                FirstName = "Ivasyk",
                LastName = "Telesyk",
            };

            var result = await userManager.CreateAsync(adminUser, "qwerty");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, Constants.Roles.Admin);
            }
            else
            {
                throw new Exception("Failed to create admin user during seeding.");
            }

            var user = new UserEntity
            {
                UserName = "user@mal.com",
                Email = "user@mal.com",
                FirstName = "Koty",
                LastName = "Goroshko",
            };

            var userResult = await userManager.CreateAsync(user, "qwerty");

            if (userResult.Succeeded)
            {
                await userManager.AddToRoleAsync(user, Constants.Roles.User);
            }
            else
            {
                throw new Exception("Failed to create user during seeding.");
            }
        }
    }
}
