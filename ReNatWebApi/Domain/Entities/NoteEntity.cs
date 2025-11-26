using Domain.Entities.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public enum NoteStatus
{
    ToDo,
    InProgress,
    Done
}

[Table("tblNotes")]
public class NoteEntity
{
    [Key]
    public long Id { get; set; }
    [StringLength(250)]
    public string Name { get; set; } = null!;
    [StringLength(1000)]
    public string? Description { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
    public NoteStatus Status { get; set; } = NoteStatus.ToDo;  //статус за замовчуванням - ToDo
    public DateTime? Deadline { get; set; }

    [ForeignKey(nameof(Category))]
    public long CategoryId { get; set; }
    public virtual NoteCategoryEntity? Category { get; set; } = null!;

    [ForeignKey(nameof(User))]
    public long UserId { get; set; }
    public virtual UserEntity User { get; set; } = null!;
}
