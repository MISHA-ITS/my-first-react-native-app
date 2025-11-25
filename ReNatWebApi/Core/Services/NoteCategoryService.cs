using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Interfaces;
using Core.Models.NoteCategory;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Services;

public class NoteCategoryService(IAuthService authService, IMapper mapper, AppDbContext appDbContext, IImageService imageService) : INoteCategoryService
{
    public async Task<NoteCategoryItemModel> CreateAsync(NoteCategoryCreateModel model)
    {
        var userId = await authService.GetUserByIdAsync();
        var entity = mapper.Map<NoteCategoryEntity>(model);
        entity.userId = userId;
        if (model.Image is not null)
        {
            entity.Image = await imageService.SaveImageAsync(model.Image);
        }
        appDbContext.NoteCategories.Add(entity);
        await appDbContext.SaveChangesAsync();

        var result = mapper.Map<NoteCategoryItemModel>(entity);
        return result;
    }

    public async Task<List<NoteCategoryItemModel>> List()
    {
        var user = await authService.GetUserByIdAsync();

        var list = await appDbContext.NoteCategories
            .Where(x => x.userId == user)
            .ProjectTo<NoteCategoryItemModel>(mapper.ConfigurationProvider)
            .ToListAsync() ?? new List<NoteCategoryItemModel>();

        return list;
    }
}
