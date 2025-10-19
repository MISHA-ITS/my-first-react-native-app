namespace WebApiRN.Models;

public class SeederUserModel
{
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
    public string Image { get; set; } = "";
    public List<string> Roles { get; set; } = new();
}
