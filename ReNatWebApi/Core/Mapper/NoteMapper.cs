using AutoMapper;

namespace Core.Mapper;

public class NoteMapper : Profile
{
    public NoteMapper()
    {
        CreateMap<Domain.Entities.NoteEntity, Core.Models.Note.NoteItemModel>()
            .ForMember(opt => opt.CategoryName, opt =>
                opt.MapFrom(x => x.Category!.Name));

        CreateMap<Core.Models.Note.NoteCreateModel, Domain.Entities.NoteEntity>();
    }
}
