using Microsoft.AspNetCore.Http;

namespace Core.Models.Account;

public class RegisterModel
{
    /// <summary> /// Ім'я користувача
    public string FirstName { get; set; } = String.Empty;

    /// <summary> /// Прізвище користувача
    public string LastName { get; set; } = String.Empty;

    /// <summary> /// Електронна пошта користувача
    public string Email { get; set; } = String.Empty;

    /// <summary> /// Пароль пошта користувача
    public string Password { get; set; } = String.Empty;

    public IFormFile? ImageFile { get; set; } = null;
}
