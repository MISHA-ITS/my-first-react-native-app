using Core.Models.Note;

namespace Core.Interfaces;

public interface INoteService
{
    Task<NoteItemModel> CreateAsync(NoteCreateModel model);
    Task<List<NoteItemModel>> ListAsync();
}
