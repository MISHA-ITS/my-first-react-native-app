using Core.Models.User;

namespace Core.Interfaces;

public interface IUserService
{
    Task<UserProfileModel> GetUserByIdAsync(long userId);
    Task<List<UserProfileModel>> GetAllUsersAsync();
}
