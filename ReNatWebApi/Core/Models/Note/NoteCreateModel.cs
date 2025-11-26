namespace Core.Models.Note;

public class NoteCreateModel
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime? Deadline { get; set; }
    public long CategoryId { get; set; }
}
