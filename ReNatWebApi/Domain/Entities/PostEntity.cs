using Domain.Entities.Identity;

namespace Domain.Entities;

public class PostEntity
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public DateTime DateCreated { get; set; } = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);

    // 🔗 Зв’язок з користувачем
    public long UserId { get; set; }
    public UserEntity User { get; set; } = null!;
}
