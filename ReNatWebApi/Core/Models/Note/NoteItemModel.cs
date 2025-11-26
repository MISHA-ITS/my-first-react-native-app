using Domain.Entities;

namespace Core.Models.Note;

public class NoteItemModel
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime? Deadline { get; set; }
    public NoteStatus Status { get; set; }
    public long CategoryId { get; set; }
    public string CategoryName { get; set; } = null!;
}
