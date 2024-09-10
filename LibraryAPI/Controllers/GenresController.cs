using LibraryAPI.DAL;
using LibraryAPI.DTOs;
using LibraryAPI.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly AppDbContext context;

        public GenresController(AppDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await context.Genres.ToListAsync();
            List<GenreGetDTO> result = new List<GenreGetDTO>();
            foreach (var item in data)
            {
                GenreGetDTO dto = new GenreGetDTO(item.Id, item.Name);
               result.Add(dto);
            }
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (id < 1) return BadRequest();
            var data=await context.Genres.FirstOrDefaultAsync(g=>g.Id == id);
            if (data == null) return NotFound();
            GenreGetDTO dto = new GenreGetDTO(data.Id, data.Name);
            return Ok(dto);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GenreCreateDTO dto)
        {
            if(dto == null) return BadRequest();
            if (string.IsNullOrEmpty(dto.Name)) return BadRequest();
            Genre genre = new Genre()
            {
                Name = dto.Name,
                CreateDate = DateTime.Now,
                ModifyDate = DateTime.Now,
                IsDeleted = false
            };
            await context.Genres.AddAsync(genre);
            await context.SaveChangesAsync();
            return Created(new Uri($"/api/genres/{genre.Id}", UriKind.Relative), genre);

        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(int id,  [FromBody] GenreUpdateDTO dto)
        {
            if (dto == null) return BadRequest();
            if (string.IsNullOrEmpty(dto.Name)) return BadRequest();
            var data = await context.Genres.FirstOrDefaultAsync(g => g.Id == id);
            if (data == null) return NotFound();
            data.ModifyDate= DateTime.Now;
            data.Name = dto.Name;
            await context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if(id<1) return BadRequest();
            var data = await context.Genres.FirstOrDefaultAsync(g => g.Id == id);
            if (data == null) return NotFound();
            context.Genres.Remove(data);
            await context.SaveChangesAsync();
            return NoContent();
        }
       
    }
}
