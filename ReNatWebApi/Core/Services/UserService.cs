using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Interfaces;
using Core.Models.User;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Core.Services;

public class UserService(AppDbContext appDbContext,
IMapper mapper) : IUserService
{
    public async Task<UserProfileModel> GetUserByIdAsync(long userId)
    {
        var user = await appDbContext
            .Users
            .Where(x => x.Id == userId)
            .ProjectTo<UserProfileModel>(mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();

        return user!;
    }

    public async Task<List<UserProfileModel>> GetAllUsersAsync()
    {
        var users = await appDbContext
            .Users
            .ProjectTo<UserProfileModel>(mapper.ConfigurationProvider)
            .ToListAsync();

        // Додаємо повний URL до зображення
        foreach (var user in users)
        {
            if (!string.IsNullOrEmpty(user.Image))
            {
                user.Image = $"/images/{user.Image}";
            }
        }

        return users;
    }
}
