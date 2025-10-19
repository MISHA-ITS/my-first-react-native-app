namespace WebApiRN.Models;

public class UserModel
{
    public long Id { get; set; }
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Image { get; set; } = null!;
}
