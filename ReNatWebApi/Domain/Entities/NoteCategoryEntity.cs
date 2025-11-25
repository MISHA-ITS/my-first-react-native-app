using Domain.Entities.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("tblNoteCategories")]
public class NoteCategoryEntity
{
    [Key]
    public long Id { get; set; }
    [StringLength(250)]
    public string Name { get; set; } = null!;
    public bool IsDeleted { get; set; }

    [StringLength(150)]
    public string Image { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(User))]
    public long userId { get; set; }
    public virtual UserEntity? User { get; set; } = null!;

    public virtual ICollection<NoteEntity>? Notes { get; set; }
}
