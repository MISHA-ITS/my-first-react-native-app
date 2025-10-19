using WebApiRN.Data.Entities.Identity;

namespace WebApiRN.Interfaces;

public interface IJwtTokenService
{
    Task<string> CreateTokenAsync(UserEntity user);
}
