using Microsoft.AspNetCore.Identity;

namespace WebApiRN.Data.Entities.Identity;

public class UserLoginEntity : IdentityUserLogin<long>
{
    public virtual UserEntity User { get; set; }
}
