using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Interfaces;
using Core.Models.Note;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Services;

public class NoteService(
    IAuthService authService,
    IMapper mapper,
    AppDbContext appDbContext
) : INoteService
{
    public async Task<NoteItemModel> CreateAsync(NoteCreateModel model)
    {
        var userId = await authService.GetUserByIdAsync();

        var entity = mapper.Map<NoteEntity>(model);
        entity.UserId = userId;

        appDbContext.Notes.Add(entity);
        await appDbContext.SaveChangesAsync();

        var result = await appDbContext.Notes
            .Where(x => x.Id == entity.Id)
            .ProjectTo<NoteItemModel>(mapper.ConfigurationProvider)
            .FirstAsync();

        return result;
    }

    public async Task<List<NoteItemModel>> ListAsync()
    {
        var userId = await authService.GetUserByIdAsync();

        var list = await appDbContext.Notes
            .Where(x => x.UserId == userId && x.IsDeleted == false)
            .ProjectTo<NoteItemModel>(mapper.ConfigurationProvider)
            .ToListAsync();

        return list;
    }
}
