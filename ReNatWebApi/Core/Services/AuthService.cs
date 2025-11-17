using Core.Interfaces;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Core.Services;

public class AuthService(IHttpContextAccessor httpContextAccessor,
    UserManager<UserEntity> userManager) : IAuthService
{
    public async Task<long> GetUserByIdAsync()
    {
        var email = httpContextAccessor
            .HttpContext?
            .User?
            .FindFirst(ClaimTypes.Email)?
            .Value;
        if (string.IsNullOrEmpty(email))
            throw new UnauthorizedAccessException("User is not authenticated");
        var user = await userManager.FindByEmailAsync(email);

        return user.Id;
    }
}
