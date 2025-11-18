namespace Core.Models.User;

public class UserModel
{
    public int Id { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Image { get; set; }
    public string DateRegister { get; set; } = String.Empty;
    public string[] Roles { get; set; } = [];
}
