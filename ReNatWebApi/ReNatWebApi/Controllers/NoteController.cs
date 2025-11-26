using Core.Interfaces;
using Core.Models.Note;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ReNatWebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class NotesController(INoteService noteService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] NoteCreateModel model)
        {
            var result = await noteService.CreateAsync(model);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var items = await noteService.ListAsync();
            return Ok(items);
        }
    }
}
