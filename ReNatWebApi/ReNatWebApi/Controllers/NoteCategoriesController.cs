using Core.Interfaces;
using Core.Models.NoteCategory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ReNatWebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]

    public class NoteCategoriesController(INoteCategoryService noteCategoryService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] NoteCategoryCreateModel model)
        {
            var result = await noteCategoryService.CreateAsync(model);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var items = await noteCategoryService.List();
            return Ok(items);
        }
    }
}
