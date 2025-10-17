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
                await roleManager.CreateAsync(new RoleEntity (role));
            }
        }
    }
}
